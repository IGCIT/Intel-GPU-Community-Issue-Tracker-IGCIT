using System;

namespace IGCIT_Helper.Events {
    class DialogCancelEvent {
        public static event Action<DialogCancelEventArgs> OnDialogCancelClick;

        public static void DialogCancelClicked(DialogCancelEventArgs args) {
            OnDialogCancelClick?.Invoke(args);
        }
    }
}
