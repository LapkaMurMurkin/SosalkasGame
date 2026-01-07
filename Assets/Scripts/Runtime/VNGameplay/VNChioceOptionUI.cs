using Template.UI;
using TMPro;

namespace SosalkasGame.Runtime.VNGameplay
{
    public class VNChioceOptionUI : UIElement
    {
        public TextMeshProUGUI Text;

        public void Initialize(string optionText)
        {
            Text.text = optionText;
        }
    }
}