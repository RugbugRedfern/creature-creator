// Menus
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class InputDialog : Dialog<InputDialog>
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI placeholderText;
        [SerializeField] private TextMeshProUGUI submitText;
        [SerializeField] private TextMeshProUGUI cancelText;
        [SerializeField] private TMP_InputField inputFieldText;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;
        #endregion

        #region Methods
        public static void Input(string title = "Title", string placeholder = "Enter text...", string submit = "Submit", string cancel = "Cancel", UnityAction<string> submitEvent = null, UnityAction<string> cancelEvent = null)
        {
            Instance.titleText.text = title;
            Instance.placeholderText.text = placeholder;
            Instance.submitText.text = submit;
            Instance.cancelText.text = cancel;
            Instance.titleText.text = title;
            Instance.inputFieldText.text = "";

            Instance.submitButton.onClick.RemoveAllListeners();
            Instance.cancelButton.onClick.RemoveAllListeners();
            Instance.submitButton.onClick.AddListener(delegate
            {
                Instance.Close();
                submitEvent?.Invoke(Instance.inputFieldText.text);
            });
            Instance.cancelButton.onClick.AddListener(delegate
            {
                Instance.Close();
                cancelEvent?.Invoke(Instance.inputFieldText.text);
            });
            Instance.ignoreButton.onClick = Instance.cancelButton.onClick;

            Instance.Open();
        }
        #endregion
    }
}