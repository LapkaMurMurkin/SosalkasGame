namespace MyFirstVisualNovel.Runtime.VNGameplay
{
    public class VNFrame
    {
        public string BackgroundImageID;
        public string CharacterImageID;
        public string CharacterNameID;
        public string MainText;
        public string[][] Choice;
        public string AnchorID;
        public string JumpToAnchorID;

        public VNFrame Copy()
        {
            return new VNFrame
            {
                BackgroundImageID = this.BackgroundImageID,
                CharacterImageID = this.CharacterImageID,
                CharacterNameID = this.CharacterNameID,
                Choice = this.Choice,
                AnchorID = this.AnchorID,
                JumpToAnchorID = this.JumpToAnchorID,
                MainText = this.MainText
            };
        }
    }
}