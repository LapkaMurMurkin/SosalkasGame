using System;
using SosalkasGame.Runtime.Core;
using SosalkasGame.Runtime.Core.AssetStorage;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using VContainer.Unity;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity
{
    public class TestInteractiveEntryPoint : IInitializable, IDisposable
    {
        private TestInteractiveLifeTimeScope _testInteractiveLifeTimeScope;
        private GameFSM _gameFSM;
        private TestInteractive _testInteractive;
        private TestInteractiveUI _testInteractiveUI;
        private AssetStorage _assetStorage;
        private ActionMap _actionMap;
        private UIRoot _uiRoot;

        public TestInteractiveEntryPoint(TestInteractiveLifeTimeScope testInteractiveLifeTimeScope, GameFSM gameFSM, UIRoot uiRoot, AssetStorage assetStorage, ActionMap actionMap)
        {
            _testInteractiveLifeTimeScope = testInteractiveLifeTimeScope;
            _gameFSM = gameFSM;
            _testInteractive = new TestInteractive(this);
            _testInteractiveUI = assetStorage.InstantiateAsset<TestInteractiveUI>(AssetID.UI_TEST_INTERACTIVE);
            _assetStorage = assetStorage;
            _actionMap = actionMap;
            _uiRoot = uiRoot;
        }

        public void Initialize()
        {
            _testInteractiveUI.Initialize(_gameFSM.GetState<VNInteractivityState>(), _testInteractive, _actionMap);
            _uiRoot.AddScreen(_testInteractiveUI.gameObject);
        }

        public void Dispose()
        {
            GameObject.Destroy(_testInteractiveUI.gameObject);
        }

        public void EndInteraction()
        {
            _testInteractiveLifeTimeScope.InteractionEnded.Invoke();
        }
    }
}