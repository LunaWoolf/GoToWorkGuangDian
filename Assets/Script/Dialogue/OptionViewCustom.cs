using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

using Yarn.Unity;

    public class OptionViewCustom : UnityEngine.UI.Selectable, ISubmitHandler, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] TextMeshProUGUI tm;
        [SerializeField] Image background;
        [SerializeField] bool showCharacterName = false;

        [SerializeField] float highlightSpeed = 0.5f;


        public Action<DialogueOptionCustom> OnOptionSelected;

        DialogueOptionCustom _option;

        bool hasSubmittedOptionSelection = false;
        public bool _isHighlight = false;
        public bool _isEssentialOption = false;

        public string _cleanOptionText = "Defualt Option";

    public DialogueOptionCustom Option
    {
            get => _option;

            set
            {
                _option = value;

                hasSubmittedOptionSelection = false;

                InternalParseForOption(value);

            }
    }

    // If we receive a submit or click event, invoke our "we just selected
    // this option" handler.

    public void InternalParseForOption(DialogueOptionCustom value)
    {
        
        if (showCharacterName)
        {
            _cleanOptionText = value.Line.Text.Text;
        }
        else
        {
            _cleanOptionText = value.Line.TextWithoutCharacterName.Text;
        }

        if (value.Line.Text.Text.Contains("<E>"))
        {
            _isEssentialOption = true;
            _cleanOptionText = value.Line.Text.Text.Replace("<E>", "");
        }

        tm.text = _cleanOptionText;
        interactable = value.IsAvailable;

        if (_isEssentialOption)
        {
            LocalDialogueManager.instance.StartEssentialOptionTransition();
        }
    }



        public void OnSubmit(BaseEventData eventData)
        {
            InvokeOptionSelected();
        }

        public void InvokeOptionSelected()
        {
           
            if (hasSubmittedOptionSelection == false)
            {
                OnOptionSelected.Invoke(Option);
                hasSubmittedOptionSelection = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            InvokeOptionSelected();
        }

        // If we mouse-over, we're telling the UI system that this element is
        // the currently 'selected' (i.e. focused) element. 
        public override void OnPointerEnter(PointerEventData eventData)
        {
            //base.Select();
            isHighlight = true;
        }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isHighlight = false;
    }

    public bool isHighlight
    {
        get { return _isHighlight; }
        set
        {
            if (_isHighlight != value)
            {
                _isHighlight = value;
                if (isHighlight)
                {
                    LeanTween.value(this.gameObject, Color.black, Color.white, highlightSpeed).setOnUpdate((Color val) => { if (tm) tm.color = val; });
                    LeanTween.value(gameObject, Color.clear, Color.black, highlightSpeed).setOnUpdate((Color val) => { if (background) background.color = val; });
                }
                else
                {

                    LeanTween.value(gameObject, Color.white, Color.black, highlightSpeed).setOnUpdate((Color val) => { if (tm) tm.color = val; });
                    LeanTween.value(this.gameObject, Color.black, Color.clear, highlightSpeed).setOnUpdate((Color val) => { if (background) background.color = val; });
                }
            }
        }

    }
}
