using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using Yarn.Unity;

public class Character : MonoBehaviour {
    [System.Serializable] // texture lookup for facial expressions
    public class Expression {
        public string name;
        public Sprite sprite; 
    }

    // objects needed to render new textures on Character face
    [SerializeField] List<Expression> expressions = new List<Expression>();
    [SerializeField] Image faceImage; 

    // object needed to set Character pose
    private Animator animator;

    // when this character is first created
    public void Awake() {
        // set initial facial expression and pose to defaults
        var defaultExpression = expressions[0].sprite;
        faceImage.sprite = defaultExpression;
        animator = GetComponentInChildren<Animator>();
    }

    // tell the animator to jump to state {poseName} 
    [YarnCommand("pose")]
    public void SetPose(string poseName) {
        string stateName = "Base Layer." + poseName;
        Debug.Log($"{name} playing {stateName}");
        animator.Play(stateName, 0);
    }

    // sets character expression texture to {expressionName} texture
    [YarnCommand("expression")]
    public void SetExpression(string expressionName){
        // find the expression with the same name as we are looking for
        Expression expressionToUse = null;
        foreach (var expression in expressions) {
            if (expression.name.Equals(expressionName, System.StringComparison.InvariantCultureIgnoreCase)) {
                expressionToUse = expression;
                break;
            }
        }
        // get the faceRenderer to apply the expression texture to the Character's face
        if (expressionToUse != null) {
            faceImage.sprite = expressionToUse.sprite;
        }
    }
}