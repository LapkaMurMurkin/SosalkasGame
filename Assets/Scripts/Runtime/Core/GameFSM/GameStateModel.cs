using MyFirstVisualNovel.Runtime.VNGameplay;

namespace MyFirstVisualNovel.Runtime.Core.GameFSM
{
    public class GameStateModel
    {
        public string CurrentScenarioConfigPath;
        public string[][] VariablesTable;
        public string[][] ScenarioTable;
        public VNFrame[] Frames;
        public int CurrentFrameIndex;
    }
}