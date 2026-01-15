using Extensions;
using SosalkasGame.Runtime.Core.AssetStorage;
using SosalkasGame.Runtime.Core.GameFSM;
using SosalkasGame.Runtime.VNGameplay;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SosalkasGame.Runtime.VNGameplay
{
    public class VNGameplayUI : MonoBehaviour
    {
        private GameStateModel _gameStateModel;
        private AssetStorage _assetStorage;
        private VNFrame _loadedFrame;

        public RawImage BackgroundImage;
        public RawImage CharacterImage;
        public TextMeshProUGUI CharacterName;
        public TextMeshProUGUI MainText;
        public TMP_FontAsset CurrentFont;

        public VNChoiceUI ChoiceMenu;
        private TMPTagAnimator _tmpTagAnimator;

        public void Initialize(GameStateModel gameStateModel, AssetStorage assetStorage)
        {
            _gameStateModel = gameStateModel;
            _assetStorage = assetStorage;
            MainText.font = CurrentFont;
            ChoiceMenu.Initialize();
            _tmpTagAnimator = new TMPTagAnimator(MainText);
        }

        public void Update()
        {
            if (_gameStateModel is null)
                return;
            if (_gameStateModel.Frames is null)
                return;
            if (_gameStateModel.CurrentFrame is null)
                return;
            if (_loadedFrame == _gameStateModel.CurrentFrame)
                return;

            ShowFrame(_gameStateModel.CurrentFrame);
        }

        public void ShowFrame(VNFrame frame)
        {
            _loadedFrame = frame;
            BackgroundImage.texture = _assetStorage.GetAssetRef<Texture2D>(_loadedFrame.BackgroundImageID);
            CharacterImage.texture = _assetStorage.GetAssetRef<Texture2D>(_loadedFrame.CharacterImageID);
            CharacterName.text = _loadedFrame.CharacterNameID;
            MainText.text = _loadedFrame.MainText;
        }
    }
}