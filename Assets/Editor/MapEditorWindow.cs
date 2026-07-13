using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEditorWindow : EditorWindow
{
    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    private enum Step { Dimensions, Paint, Done }
    private Step _step = Step.Dimensions;

    // Step 1
    private int _mapWidth  = 10;
    private int _mapHeight = 10;

    // Step 2
    private CellData[,] _cells;

    // Painting
    private bool _isPainting = false;
    private bool _paintingWalkable;

    // Scroll
    private Vector2 _scroll;

    // Prefab settings (used instead of bare GameObjects when generating the scene)
    private GameObject _cellPrefab;

    // Extra prefabs the user wants dropped into the generated scene (camera, managers, etc.)
    private List<GameObject> _extraPrefabs = new List<GameObject>();
    private bool _extraPrefabsFoldout = true;
    private Vector2 _extraPrefabsScroll;

    // Layout constants
    private const int CellPx     = 32;
    private const int CellMargin = 1;

    // -------------------------------------------------------------------------
    // Colors per terrain
    // -------------------------------------------------------------------------

    private static readonly Dictionary<CellTerrain, Color> TerrainColors = new Dictionary<CellTerrain, Color>
    {
        { CellTerrain.none,     new Color(0.3f, 0.3f, 0.3f) },
        { CellTerrain.grass,    new Color(0.3f, 0.7f, 0.2f) },
        { CellTerrain.water,    new Color(0.2f, 0.4f, 0.9f) },
        { CellTerrain.mountain, new Color(0.5f, 0.4f, 0.3f) },
    };

    private static readonly CellTerrain[] TerrainCycle =
    {
        CellTerrain.grass,
        CellTerrain.water,
        CellTerrain.mountain,
        CellTerrain.none,
    };

    // -------------------------------------------------------------------------
    // Open window
    // -------------------------------------------------------------------------

    [MenuItem("Tools/Map Editor")]
    public static void Open()
    {
        MapEditorWindow window = GetWindow<MapEditorWindow>("Map Editor");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    // -------------------------------------------------------------------------
    // GUI
    // -------------------------------------------------------------------------

    private void OnGUI()
    {
        if (_step == Step.Dimensions)
        {
            DrawDimensionsStep();
        }
        else if (_step == Step.Paint)
        {
            DrawPaintStep();
        }
    }

    // ---- Step 1: Dimensions ----

    private void DrawDimensionsStep()
    {
        GUILayout.Space(20);
        GUILayout.Label("New Map", EditorStyles.boldLabel);
        GUILayout.Space(10);

        _mapWidth  = EditorGUILayout.IntField("Width",  _mapWidth);
        _mapHeight = EditorGUILayout.IntField("Height", _mapHeight);

        _mapWidth  = Mathf.Clamp(_mapWidth,  1, 50);
        _mapHeight = Mathf.Clamp(_mapHeight, 1, 50);

        GUILayout.Space(20);

        if (GUILayout.Button("Create Grid", GUILayout.Height(36)))
        {
            InitCells();
            _step = Step.Paint;
        }
    }

    private void InitCells()
    {
        _cells = new CellData[_mapWidth, _mapHeight];

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                _cells[x, y] = new CellData
                {
                    terrain   = CellTerrain.grass,
                    isWalkable = true
                };
            }
        }
    }

    // ---- Step 2: Paint ----

    private void DrawPaintStep()
    {
        // Toolbar
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label($"Map  {_mapWidth} x {_mapHeight}", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Right-click: cycle terrain   |   Left-click: toggle walkable", EditorStyles.miniLabel);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("← Back", EditorStyles.toolbarButton, GUILayout.Width(60)))
        {
            _step = Step.Dimensions;
        }

        if (GUILayout.Button("Generate Scene", EditorStyles.toolbarButton, GUILayout.Width(110)))
        {
            GenerateScene();
        }

        EditorGUILayout.EndHorizontal();

        // Prefab settings
        DrawPrefabSettings();

        // Legend
        DrawLegend();

        // Grid scroll area
        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        Rect gridRect = GUILayoutUtility.GetRect(
            _mapWidth  * (CellPx + CellMargin),
            _mapHeight * (CellPx + CellMargin));

        DrawGrid(gridRect);
        HandleGridInput(gridRect);

        EditorGUILayout.EndScrollView();
    }

    // ---- Prefab settings UI ----

    private void DrawPrefabSettings()
    {
        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("Prefab Settings", EditorStyles.boldLabel);

        // Cell prefab - used instead of a bare GameObject for every cell
        EditorGUI.BeginChangeCheck();
        GameObject newCellPrefab = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Cell Prefab", "Optional. If set, this prefab is instantiated for every cell instead of creating an empty GameObject. Must be a prefab asset (not a scene object)."),
            _cellPrefab,
            typeof(GameObject),
            false);

        if (EditorGUI.EndChangeCheck())
        {
            if (newCellPrefab != null && !IsPrefabAsset(newCellPrefab))
            {
                Debug.LogWarning("[MapEditor] Cell Prefab must be a prefab asset from your project, not a scene object.");
            }
            else
            {
                _cellPrefab = newCellPrefab;
            }
        }

        EditorGUILayout.Space(4);

        // Extra prefabs list - anything else the user wants dropped into the generated scene
        _extraPrefabsFoldout = EditorGUILayout.Foldout(_extraPrefabsFoldout, "Extra Objects To Place In Scene (camera, managers, lights, etc.)", true);

        if (_extraPrefabsFoldout)
        {
            EditorGUI.indentLevel++;

            if (_extraPrefabs.Count == 0)
            {
                EditorGUILayout.HelpBox("No extra objects added yet. Click \"+\" to add a prefab to spawn alongside the grid.", MessageType.None);
            }

            _extraPrefabsScroll = EditorGUILayout.BeginScrollView(_extraPrefabsScroll, GUILayout.MaxHeight(140));

            for (int i = 0; i < _extraPrefabs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GameObject newValue = (GameObject)EditorGUILayout.ObjectField(_extraPrefabs[i], typeof(GameObject), false);

                if (newValue != _extraPrefabs[i])
                {
                    if (newValue != null && !IsPrefabAsset(newValue))
                    {
                        Debug.LogWarning("[MapEditor] Extra objects must be prefab assets from your project, not scene objects.");
                    }
                    else
                    {
                        _extraPrefabs[i] = newValue;
                    }
                }

                if (GUILayout.Button("-", GUILayout.Width(24)))
                {
                    _extraPrefabs.RemoveAt(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("+ Add Object", GUILayout.Width(110)))
            {
                _extraPrefabs.Add(null);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private static bool IsPrefabAsset(GameObject go)
    {
        return PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab
            && !go.scene.IsValid();
    }

    private void DrawLegend()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(8);

        foreach (KeyValuePair<CellTerrain, Color> kvp in TerrainColors)
        {
            Color prev = GUI.color;
            GUI.color = kvp.Value;
            GUILayout.Box("", GUILayout.Width(14), GUILayout.Height(14));
            GUI.color = prev;
            GUILayout.Label(kvp.Key.ToString(), EditorStyles.miniLabel);
            GUILayout.Space(8);
        }

        // Non-walkable indicator
        Color prevCol = GUI.color;
        GUI.color = Color.red;
        GUILayout.Box("X", GUILayout.Width(14), GUILayout.Height(14));
        GUI.color = prevCol;
        GUILayout.Label("non-walkable", EditorStyles.miniLabel);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(4);
    }

    private void DrawGrid(Rect gridRect)
    {
        if (_cells == null) return;

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                CellData data = _cells[x, y];
                Rect cellRect = GetCellRect(gridRect, x, y);

                // Background color from terrain
                EditorGUI.DrawRect(cellRect, TerrainColors[data.terrain]);

                // Non-walkable overlay
                if (!data.isWalkable)
                {
                    Rect inner = new Rect(cellRect.x + 4, cellRect.y + 4, cellRect.width - 8, cellRect.height - 8);
                    EditorGUI.DrawRect(inner, new Color(1f, 0f, 0f, 0.6f));
                }

                // Thin dark border
                DrawCellBorder(cellRect, new Color(0f, 0f, 0f, 0.4f));
            }
        }
    }

    private void HandleGridInput(Rect gridRect)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0) _isPainting = true;
        }

        if (e.type == EventType.MouseUp)
        {
            _isPainting = false;
        }

        bool isClick = e.type == EventType.MouseDown;
        bool isDrag  = e.type == EventType.MouseDrag && _isPainting;

        if (!isClick && !isDrag) return;
        if (!gridRect.Contains(e.mousePosition)) return;

        int x = Mathf.FloorToInt((e.mousePosition.x - gridRect.x) / (CellPx + CellMargin));
        int drawY = Mathf.FloorToInt((e.mousePosition.y - gridRect.y) / (CellPx + CellMargin));
        int y = (_mapHeight - 1) - drawY;

        if (x < 0 || x >= _mapWidth || y < 0 || y >= _mapHeight) return;

        if (isClick && e.button == 1)
        {
            // Right click: cycle terrain
            CellTerrain current = _cells[x, y].terrain;
            int index = System.Array.IndexOf(TerrainCycle, current);
            _cells[x, y].terrain = TerrainCycle[(index + 1) % TerrainCycle.Length];
            e.Use();
            Repaint();
        }
        else if (e.button == 0)
        {
            // Left click / drag: toggle walkable (on first click, record the target state)
            if (isClick)
            {
                _paintingWalkable = !_cells[x, y].isWalkable;
            }

            _cells[x, y].isWalkable = _paintingWalkable;
            e.Use();
            Repaint();
        }
    }

    // -------------------------------------------------------------------------
    // Scene generation
    // -------------------------------------------------------------------------

    private void GenerateScene()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        // Create and save a new scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // TacticalGrid root
        GameObject gridRoot = new GameObject("TacticalGrid");
        TacticalGrid tacticalGrid = gridRoot.AddComponent<TacticalGrid>();

        // One GameObject (or prefab instance) per cell
        GameObject cellsRoot = new GameObject("Cells");
        cellsRoot.transform.SetParent(gridRoot.transform);

        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                CellData data = _cells[x, y];
                CreateCellObject(x, y, data, cellsRoot.transform);
            }
        }

        // Extra objects (camera, managers, lights, etc.) requested by the user
        CreateExtraObjects(gridRoot.transform);

        string path = EditorUtility.SaveFilePanelInProject("Save Map Scene", "NewMap", "unity", "Choose where to save the map scene.");

        if (string.IsNullOrEmpty(path)) return;

        EditorSceneManager.SaveScene(newScene, path);
        Debug.Log($"[MapEditor] Scene saved to {path} with {_mapWidth * _mapHeight} cells.");
    }

    private void CreateCellObject(int x, int y, CellData data, Transform parent)
    {
        GameObject cellGO;

        if (_cellPrefab != null)
        {
            // Instantiate the user-provided prefab, keeping the prefab link
            cellGO = (GameObject)PrefabUtility.InstantiatePrefab(_cellPrefab, parent);
        }
        else
        {
            // Fallback: plain GameObject, same as before
            cellGO = new GameObject();
            cellGO.transform.SetParent(parent);
        }

        cellGO.name = $"Cell_{x}_{y}";
        cellGO.transform.position = new Vector3(x, y, 0f);

        // Reuse components already on the prefab if present, otherwise add them
        GridCellMono mono = cellGO.GetComponent<GridCellMono>();
        if (mono == null) mono = cellGO.AddComponent<GridCellMono>();

        GridCellSetup setup = cellGO.GetComponent<GridCellSetup>();
        if (setup == null) setup = cellGO.AddComponent<GridCellSetup>();
        setup.terrain    = data.terrain;
        setup.isWalkable = data.isWalkable;

        SpriteRenderer sr = cellGO.GetComponent<SpriteRenderer>();
        if (sr == null) sr = cellGO.AddComponent<SpriteRenderer>();
        if (sr.sprite == null) sr.sprite = GetDefaultSprite();
        sr.color = TerrainColors[data.terrain];

        if (cellGO.GetComponent<BoxCollider2D>() == null)
        {
            cellGO.AddComponent<BoxCollider2D>();
        }
    }

    private void CreateExtraObjects(Transform gridRoot)
    {
        List<GameObject> valid = _extraPrefabs.FindAll(p => p != null);
        if (valid.Count == 0) return;

        GameObject extrasRoot = new GameObject("Extras");
        extrasRoot.transform.SetParent(gridRoot);

        foreach (GameObject prefab in valid)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, extrasRoot.transform);
            instance.name = prefab.name;
        }
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private Rect GetCellRect(Rect gridRect, int x, int y)
    {
        // Y is flipped so row 0 is at the bottom like Unity's world space
        int drawY = (_mapHeight - 1) - y;

        return new Rect(
            gridRect.x + x     * (CellPx + CellMargin),
            gridRect.y + drawY * (CellPx + CellMargin),
            CellPx,
            CellPx);
    }

    private void DrawCellBorder(Rect rect, Color color)
    {
        EditorGUI.DrawRect(new Rect(rect.x,                  rect.y,                   rect.width, 1),          color);
        EditorGUI.DrawRect(new Rect(rect.x,                  rect.y + rect.height - 1, rect.width, 1),          color);
        EditorGUI.DrawRect(new Rect(rect.x,                  rect.y,                   1,          rect.height), color);
        EditorGUI.DrawRect(new Rect(rect.x + rect.width - 1, rect.y,                   1,          rect.height), color);
    }

    private Sprite GetDefaultSprite()
    {
        // Returns Unity's built-in white square sprite — replace with your own tile sprite if needed
        return AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
    }

    // -------------------------------------------------------------------------
    // Data
    // -------------------------------------------------------------------------

    private class CellData
    {
        public CellTerrain terrain;
        public bool        isWalkable;
    }
}