using System.Collections.Generic;
using MyFirstVisualNovel.Runtime.Core.UI;
using MyFirstVisualNovel.Runtime.VNGameplay;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class GameStateModel
    {
        public AssetStorage.AssetStorage AssetStorage;
        public SceneLoader.SceneLoader SceneLoader;
        public ActionMap ActionMap;
        public UIRoot UIRoot;

        public string ScenarioConfigPath;
        public string[][] VariablesTable;
        public string[][] ScenarioTable;
        public VNFrame[] Frames;
        public int FrameIndex;
        public Dictionary<string, int> Anchors;

        public List<int> FramesHistory;

        public VNFrame CurrentFrame => Frames[FrameIndex];
    }
}