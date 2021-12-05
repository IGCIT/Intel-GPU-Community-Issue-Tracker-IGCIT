namespace IGCIT_Helper.Events {
    class DialogCancelEventArgs {
        public DialogType EventType { get; private set; }
        public string Token { get; private set; }

        public DialogCancelEventArgs(DialogType t, string tok) {
            EventType = t;
            Token = tok;
        }
    }
}
