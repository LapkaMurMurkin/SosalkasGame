using System.Collections.Generic;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.Core.GameEntryPoint;
using SosalkasGame.Runtime.VNGameplay;

namespace SosalkasGame.Runtime.Core.GameFSM
{
    public class GameStateModel
    {
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