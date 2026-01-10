using Extensions;
using MyFirstVisualNovel.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.VNGameplay;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNGameplayUI : MonoBehaviour
    {
        private GameStateModel _gameStateModel;
        private VNFrame _loadedFrame;

        public RawImage BackgroundImage;
        public RawImage CharacterImage;
        public TextMeshProUGUI CharacterName;
        public TextMeshProUGUI MainText;
        public TMP_FontAsset CurrentFont;

        public VNChoiceUI ChoiceMenu;
        private TMPAnimation _tmpAnimation;

        public void Initialize(GameStateModel gameStateModel)
        {
            _gameStateModel = gameStateModel;
            MainText.font = CurrentFont;
            ChoiceMenu.Initialize();
            _tmpAnimation = new TMPAnimation(MainText);
        }

        public void Update()
        {
            //if(_gameState.CurrentState is VNInteractiveState)
            _tmpAnimation.Update();

            if (_gameStateModel is null)
                return;
            if (_gameStateModel.Frames is null)
                return;
            if (_gameStateModel.CurrentFrame is null)
                return;
            if (_loadedFrame == _gameStateModel.CurrentFrame)
                return;

            ShowFrame(_gameStateModel.CurrentFrame);
            _tmpAnimation.ForceTMPMeshUpdate();
        }



        public void ShowFrame(VNFrame frame)
        {
            _loadedFrame = frame;
            BackgroundImage.texture = _gameStateModel.AssetStorage.GetAssetRef<Texture2D>(_loadedFrame.BackgroundImageID);
            CharacterImage.texture = _gameStateModel.AssetStorage.GetAssetRef<Texture2D>(_loadedFrame.CharacterImageID);
            CharacterName.text = _loadedFrame.CharacterNameID;
            MainText.text = _loadedFrame.MainText;
        }
    }
}