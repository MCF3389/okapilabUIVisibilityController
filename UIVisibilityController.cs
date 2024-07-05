using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


/// <summary>
/// This is an extension for the software GPTAvatar created by Seth Robinson, https://github.com/SethRobinson/GPTAvatar
/// 
/// The class UIVisibilityController is part of OKAPI lab's / Maximilian C. Fink's OKAPILabExtensions for GPTAvatar
/// The class allows to hide/show the buttons/labels with a "GUI on/off" Button 
/// This extension can be used in experimental settings where participant's should not see the full GUI all the time
/// It should work on the desktop version of GPTAvatar, but potentially also on its WebGL version.
/// 
/// To use this extension, the following steps have to be done
/// 1) Copy this file in Unity somehwere under your assets (e.g., in a separate folder OKAPILabExtensions)
/// 2) Click in the Unity hierarchy on "Canvas" and open the child object "Panel"
/// 3) Click on the "Panel" component on "Add Component" and then type/select "UIVisibilityController" -> this adds the C# file
/// Now everything should run as intended. No further changes should be neccessary. Try it in play mode and make builds :)
/// 
/// If you use GPTAvatar for psychological/educational research, please either cite our article 
/// http://www.dx.doi.org/10.3389/feduc.2024.1416307/abstract or Seth Robinson's github repository.
/// If you need more code modification or want to collaborate for scientific purposes, please reach out to Maximilian C. Fink (maximilian.fink@yahoo.com)
/// I'm always happy to help and conduct research together :)
/// </summary>

public class UIVisibilityController : MonoBehaviour
{
    private Button dialogButtonInstance;
    private Button statusButtonInstance;
    private Button hideUIButtonInstance;

    private TextMeshProUGUI dialogButtonText;
    private TextMeshProUGUI statusButtonText;
    private TextMeshProUGUI hideUIButtonText;

    // UI Elements from GPTAvatar
    private GameObject recordButton;
    private GameObject forgetButton;
    private GameObject configButton;
    private GameObject stopButton;
    private GameObject copyButton;
    private GameObject nextButton;
    private GameObject previousButton;
    private TMP_Text friendNameText;

    private TMP_Text dialogTextToToggle;
    private TMP_Text statusTextToToggle;
    private TMP_Text hideUITextToToggle;

    private bool isDialogActive = true;
    private bool isStatusActive = true;
    private bool isUIActive = true;

    public RectTransform parentPanel;

    void Awake()
    {
        recordButton = GameObject.Find("RecordButton");
        if (recordButton != null)
        {
            CreateButtonFromExisting(recordButton, "DialogButton", "Dialog on", new Vector2(120, -535),
                out dialogButtonInstance, out dialogButtonText, OnDialogButtonClicked);
            CreateButtonFromExisting(recordButton, "StatusButton", "Status on", new Vector2(220, -535),
                out statusButtonInstance, out statusButtonText, OnStatusButtonClicked);
            CreateButtonFromExisting(recordButton, "HideUIButton", "GUI on", new Vector2(20, -535),
                out hideUIButtonInstance, out hideUIButtonText, OnHideUIButtonClicked);

            // Set the new buttons to half their size
            SetButtonSize(dialogButtonInstance, 0.7f);
            SetButtonSize(statusButtonInstance, 0.7f);
            SetButtonSize(hideUIButtonInstance, 0.7f);
        }
        else
        {
            Debug.LogError("RecordButton not found!");
        }

        dialogTextToToggle = GameObject.Find("DialogText")?.GetComponentInChildren<TMP_Text>();
        statusTextToToggle = GameObject.Find("StatusText")?.GetComponentInChildren<TMP_Text>();
        hideUITextToToggle = GameObject.Find("HideUIButton")?.GetComponentInChildren<TMP_Text>();

        // search for Game Objects and assign them
        assignUIElements();
    }

    private void assignUIElements()
    {
        forgetButton = GameObject.Find("ForgetButton");
        configButton = GameObject.Find("ConfigButton");
        stopButton = GameObject.Find("StopButton");
        copyButton = GameObject.Find("CopyButton");
        nextButton = GameObject.Find("NextButton");
        previousButton = GameObject.Find("PreviousButton");
        friendNameText = GameObject.Find("FriendNameText").GetComponent<TMP_Text>();
    }

    private void SetButtonSize(Button button, float scale)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            Debug.LogError("RectTransform component not found on the button!");
        }
    }

    private void CreateButtonFromExisting(GameObject existingButton, string buttonName, string initialText, Vector2 anchoredPosition,
        out Button button, out TextMeshProUGUI buttonText, UnityEngine.Events.UnityAction callback)
    {
        GameObject newButton = Instantiate(existingButton, parentPanel);
        newButton.name = buttonName;
        button = newButton.GetComponent<Button>();

        buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = initialText;
            buttonText.alignment = TextAlignmentOptions.Center; // Center the text
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on the new button!");
        }

        // Add a listener to the Button's onClick event
        // button.onClick.RemoveAllListeners(); -> This doesnt work. It seems we need to "kill" a persistant event listener
        // lets use this and kill the first event listener (index 0) on the onClick event! https://docs.unity3d.com/ScriptReference/Events.UnityEventBase.SetPersistentListenerState.html
        button.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
        button.onClick.AddListener(callback);

        // Debug after adding listener
        Debug.Log($"After AddListener: {button.onClick.GetPersistentEventCount()} listeners on {buttonName}");

        RectTransform buttonRect = newButton.GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            buttonRect.anchorMin = new Vector2(0, 1);
            buttonRect.anchorMax = new Vector2(0, 1);
            buttonRect.pivot = new Vector2(0, 1);
            buttonRect.anchoredPosition = anchoredPosition;
        }
        else
        {
            Debug.LogError("RectTransform component not found on the new button!");
        }
    }

    private void OnDialogButtonClicked()
    {
        isDialogActive = !isDialogActive;
        if (isDialogActive)
        {
            Debug.Log("Dialog Button clicked: ON");
            dialogButtonText.text = "Dialog on";
            ActivateDialog();
        }
        else
        {
            Debug.Log("Dialog Button clicked: OFF");
            dialogButtonText.text = "Dialog off";
            DeactivateDialog();
        }
    }

    private void OnStatusButtonClicked()
    {
        isStatusActive = !isStatusActive;
        if (isStatusActive)
        {
            Debug.Log("Status Button clicked: ON");
            statusButtonText.text = "Status on";
            ActivateStatus();
        }
        else
        {
            Debug.Log("Status Button clicked: OFF");
            statusButtonText.text = "Status off";
            DeactivateStatus();
        }
    }

    private void ActivateDialog()
    {
        Debug.Log("Dialog Activated");
        if (dialogTextToToggle != null)
        {
            dialogTextToToggle.enabled = true;
        }
        else
        {
            Debug.Log("DialogText object and TMP_Text not found! Is the Object named 'DialogText' available?");
        }
    }

    private void DeactivateDialog()
    {
        Debug.Log("Dialog Deactivated");
        if (dialogTextToToggle != null)
        {
            dialogTextToToggle.enabled = false;
        }
        else
        {
            Debug.Log("DialogText object and TMP_Text not found! Is the Object named 'DialogText' available?");
        }
    }

    private void ActivateStatus()
    {
        Debug.Log("Status Activated");
        if (statusTextToToggle != null)
        {
            statusTextToToggle.enabled = true;
        }
        else
        {
            Debug.Log("StatusText object and TMP_Text not found! Is the Object named 'StatusText' available?");
        }
    }

    private void DeactivateStatus()
    {
        Debug.Log("Status Deactivated");
        if (statusTextToToggle != null)
        {
            statusTextToToggle.enabled = false;
        }
        else
        {
            Debug.Log("StatusText object and TMP_Text not found! Is the Object named 'StatusText' available?");
        }
    }

    // Implementing a simple Toggle
    private void OnHideUIButtonClicked()
    {
        if (isUIActive)
        {
            HideUI();
            isUIActive = !isUIActive;
            hideUITextToToggle.text = "GUI off";
        }
        else
        {
            ShowUI();
            isUIActive = !isUIActive;
            hideUITextToToggle.text = "GUI on";
        }
    }

    // helper functions are below (deactivateButton) and activateButton
    private void HideUI()
    {
        if (forgetButton != null)
        {
            deactivateButton(forgetButton);
        }
        if (configButton != null)
        {
            deactivateButton(configButton);
        }
        if (stopButton != null)
        {
            deactivateButton(stopButton);
        }
        if (copyButton != null)
        {
            deactivateButton(copyButton);
        }
        if (dialogButtonInstance != null)
        {
            deactivateButton(dialogButtonInstance.gameObject);
        }
        if (statusButtonInstance != null)
        {
            deactivateButton(statusButtonInstance.gameObject);
        }
        /* outcommented - > we don't want to hide the hide button if we want a toggle functionality
         * can be changed if UI elements should be hidden persistently
         if (hideUIButtonInstance != null)
        {
            deactivateButton(hideUIButtonInstance.gameObject);
        }*/
        if (nextButton != null)
        {
            deactivateButton(nextButton);
        }
        if (previousButton != null)
        {
            deactivateButton(previousButton);
        }
        if (friendNameText != null)
        {
            friendNameText.gameObject.SetActive(false);
        }
    }

    private void ShowUI()
    {
        if (forgetButton != null)
        {
            activateButton(forgetButton);
        }
        if (configButton != null)
        {
            activateButton(configButton);
        }
        if (stopButton != null)
        {
            activateButton(stopButton);
        }
        if (copyButton != null)
        {
            activateButton(copyButton);
        }
        if (dialogButtonInstance != null)
        {
            activateButton(dialogButtonInstance.gameObject);
        }
        if (statusButtonInstance != null)
        {
            activateButton(statusButtonInstance.gameObject);
        }
        /* outcommented - > we don't want to hide the hide button if we want a toggle functionality
         * can be changed if UI elements should be hidden persistently
         if (hideUIButtonInstance != null)
        {
            deactivateButton(hideUIButtonInstance.gameObject);
        }*/
        if (nextButton != null)
        {
            activateButton(nextButton);
        }
        if (previousButton != null)
        {
            activateButton(previousButton);
        }
        if (friendNameText != null)
        {
            friendNameText.gameObject.SetActive(true);
        }
    }

    // helper function to hide buttons without setting them to inactive (deactivating their logic)
    private void deactivateButton(GameObject btn)
    {
        CanvasGroup canvasGroup = btn.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = btn.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f; // Set alpha to 0 to make it invisible
    }

    // Helper function to make buttons visible without setting them to inactive (keeping their logic active)
    private void activateButton(GameObject btn)
    {
        CanvasGroup canvasGroup = btn.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = btn.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f; // Set alpha to 1 to make it visible
    }
}
