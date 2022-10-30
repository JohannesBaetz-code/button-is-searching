using System;
using System.Collections.Generic;
using StateManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     The CardQuiz is the basic form of the Testmode, where one can iterate through different questions
///     about the currently used algorithm to learn more.
/// </summary>
/// <author>Fanny Weidner</author>
public class CardQuiz : MonoBehaviour
{
    private static CardQuiz thisCardQuiz;
    
    /// <summary> Required object references through the changes of the questions. </summary>
    [SerializeField] private GameObject _textBox;

    [SerializeField] private UnityEngine.UI.Button _questionButton;
    private StateManager _manager;

    /// <summary> Sprite inputs necessary for the feedback change. </summary>
    [SerializeField] private Sprite _exclamationmark;

    [SerializeField] private Sprite _questionmark;
    private String _preText = "Fragenkartei";

    /// <summary> Variables that handle the current question flow and input </summary>
    private Algorithm _currentAlgorithm;

    private string _currentAnswer;
    private List<string> _currentAnswers = new List<string>();
    private string _currentQuestion;
    private List<string> _currentQuestions = new List<string>();


    /// <summary> Helpful Variables </summary>
    private bool _activeQuestion; // is the question currently showing?

    private int _currentQuestionNumber; // What number is the current question and answer?

    public static CardQuiz GetInstance()
    {
        return thisCardQuiz;
    }
    
    public void Awake()
    {
        thisCardQuiz = this;
        ResetQuiz();
    }

    public void ClearTextBox()
    {
        _textBox.GetComponent<TextMeshProUGUI>().text = _preText;
    }

    public void ResetQuiz()
    {
        _currentQuestions = new List<string>();
        _currentAnswers = new List<string>();
        _currentQuestionNumber = 0;
    }

    /// <summary>
    ///     CardQuiz gets the currently active Algorithm and generates questions and answers based on player interaction with
    ///     the QuestionButton and CardField
    /// </summary>
    /// <param name="change Text"> Changes the required text in the field between question and answer. </param>
    public void changeText()
    {
        // What is the currently required set of questions and answers?
        // _currentQuestions = _manager._playStateManager.QuizManager._currentQuestions;
        // _currentAnswers = _manager._playStateManager.QuizManager._currentAnswers;

        // Always start with the question
        _currentQuestion = _currentQuestions[_currentQuestionNumber];
        _textBox.GetComponent<TextMeshProUGUI>().text = _currentQuestion;
        _questionButton.GetComponent<Image>().sprite = _questionmark;

        if (_activeQuestion == false)
        {
            // Show question after answer and change the buttonsprite
            _currentQuestion = _currentQuestions[_currentQuestionNumber];
            _textBox.GetComponent<TextMeshProUGUI>().text = _currentQuestion;
            _questionButton.GetComponent<Image>().sprite = _questionmark;
            _activeQuestion = true;
        }
        else
        {
            // Show answer after question and change the buttonsprite
            _currentAnswer = _currentAnswers[_currentQuestionNumber];
            _textBox.GetComponent<TextMeshProUGUI>().text = _currentAnswer;
            _activeQuestion = false;

            _questionButton.GetComponent<Image>().sprite = _exclamationmark;
        }
    }

    /// <summary>
    ///     Switches the currently active Question to the next one in the active questionaire.
    /// </summary>
    /// <param name="next Question"> Iterates to the next Question and answer in the list, or starts over if the list is over </param>
    public void nextQuestion()
    {
        // Switch to the next question in list
        _currentQuestionNumber++;

        if (_currentQuestionNumber >= _currentQuestions.Count) _currentQuestionNumber = 0;

        // always start with the question
        _activeQuestion = false;

        // Change UI Text to next question
        changeText();
    }

    /// <param name="current Questionaire">
    ///     Generates the currently active question and answer sets based on the active
    ///     algorithm.
    /// </param>
    public void currentQuestionaire()
    {
        _manager = StateManager.GetInstance();
        _currentAlgorithm = _manager._playStateManager.Algorithm;

        // Depending on the case, switch to a checkable int
        switch (_currentAlgorithm)
        {
            case Algorithm.Dijkstra:
                dijkstraQuestionsAnswers();
                break;
            case Algorithm.AStar:
                astarQuestionsAnswers();
                break;
            case Algorithm.BreadthFirstSearch:
                breadthFirstSearchQuestionsAnswers();
                break;
            case Algorithm.DepthFirstSearch:
                depthFirstSearchQuestionsAnswers();
                break;
        }
    }

    /// <param name="dijkstra Questions Answers"> Creates the list of questions for Dijkstra </param>
    private void dijkstraQuestionsAnswers()
    {
        _currentQuestions.Add("Was ist ein Greedy Algorithmus?");
        _currentQuestions.Add("Laufzeit von Dijkstra?");
        _currentQuestions.Add("Wie heisst die Motte, welche den Dijkstra-Algorithmus bevorzugt?");
        _currentQuestions.Add("Ist die Gewichtung der Kanten beim Dijkstra wichtig?");
        _currentQuestions.Add("Wie heisst der Erfinder des Dijkstra-Algorithmus?");
        _currentQuestions.Add("Wie gross darf die Gewichtung werden? Und wie klein?");
        _currentQuestions.Add("Was koennten die Gewichte bedeuten in der realen Welt?");
        _currentQuestions.Add("Was sind die Kosten vom Startknoten zum Startknoten?");
        _currentQuestions.Add(
            "Wie gross sind die Kosten von einem Startknoten zu einem Knoten, zu dem kein Weg fuehrt?");
        _currentQuestions.Add("Was wiegt mehr: 10kg Steine oder 10kg Federn?");

        // Answers set for Dijkstra
        _currentAnswers.Add("Ein Algorithmus der jederzeit nur die aktuell beste Loesung sucht.");
        _currentAnswers.Add("O(|Knotenzahl|^2)");
        _currentAnswers.Add("Needle");
        _currentAnswers.Add("Ja");
        _currentAnswers.Add("Edsger W. Dijkstra");
        _currentAnswers.Add("Die Gewichtung darf beliebig gross werden, sie sollte nur nicht negativ sein.");
        _currentAnswers.Add("Z.B.: Weglaenge, Fahrzeiten, Mautgebuehren, etc.");
        _currentAnswers.Add("0 - es gibt immerhin keine Entfernung zu sich selbst");
        _currentAnswers.Add("Unendlich - der Knoten bleibt unerreichbar");
        _currentAnswers.Add("Die Steine natuerlich. Federn sind doch viel leichter.");
    }

    /// <param name="AStar Questions Answers"> Creates the list of questions for AStar </param>
    private void astarQuestionsAnswers()
    {
        // Questions for AStar
        _currentQuestions.Add("Was ist ein Greedy Algorithmus? Ist der A* ein Greedy Algorithmus?");
        _currentQuestions.Add("Laufzeit von A*?");
        _currentQuestions.Add("Wie heisst die Motte, welche den A*-Algorithmus bevorzugt?");
        _currentQuestions.Add("Was unterscheidet den A* Algorithmus von uninformierten Algorithmen?");
        _currentQuestions.Add("Ist die Gewichtung der Kanten beim A* wichtig?");
        _currentQuestions.Add("Gibt es einen anderen Algorithmus, der mit denselben Heuristiken schneller arbeitet?");
        _currentQuestions.Add("Der A*-Algorithmus gilt als vollstaendig - was bedeutet dies?");
        _currentQuestions.Add("Welche Knoten untersucht der A*-Algorithmus immer zuerst?");
        _currentQuestions.Add("Was koennte ein Nachteil des A*-Algorithmus sein?");
        _currentQuestions.Add("Wie heisst die Motte, welche den A*-Algorithmus bevorzugt?");
        _currentQuestions.Add("Welche Band sang 1965: “Baby you can drive my car, yes i’m gonna be A*?”");

        // Answers for AStar
        _currentAnswers.Add("Ein Algorithmus der jederzeit nur die aktuell beste Loesung sucht - wie z.B. der A*.");
        _currentAnswers.Add("O(|Knotenzahl|^2)");
        _currentAnswers.Add("Patch");
        _currentAnswers.Add("Sie beruecksichtigt heuristische Werte");
        _currentAnswers.Add("Ja - sie werden beruecksichtigt.");
        _currentAnswers.Add("Nein");
        _currentAnswers.Add("Wenn eine Loesung existiert, wird diese auch gefunden.");
        _currentAnswers.Add(
            "Die, welche wahrscheinlich schnell zum Weg fuehren - Gewichtung und Heuristik inbegriffen.");
        _currentAnswers.Add("Benoetigt fuer verschiedene Listen sehr viel Speicher.");
        _currentAnswers.Add("Patch");
        _currentAnswers.Add("die “Beatles”");
    }

    /// <param name="BreadthFirstSearch Questions Answers"> Creates the list of questions for BreadthFirstSearch </param>
    private void breadthFirstSearchQuestionsAnswers()
    {
        // Questions for BreadthFirstSearch
        _currentQuestions.Add("Laufzeit von Breitensuche?");
        _currentQuestions.Add("Zu welche Nation fuehrt der laengste Tunnel der Erde?");
        _currentQuestions.Add("Wie heisst die Motte, welche die Breitensuche bevorzugt?");
        _currentQuestions.Add("Nimmt die Tiefensuche immer den weitesten linken oder den weitesten rechten Knoten?");
        _currentQuestions.Add("Was ist der Unterschied der Breitensuche zur Tiefensuche?");
        _currentQuestions.Add(
            "Die Breitensuche funktioniert bei ungerichtete und gerichtete Graphen - wahr oder falsch?");
        _currentQuestions.Add("Findet die Breitensuche immer den kuerzesten Weg?");
        _currentQuestions.Add("Ist die Gewichtung der Kanten bei der Breitensuche wichtig?");
        _currentQuestions.Add("Sind heuristische Werte bei der Tiefensuche wichtig?");
        _currentQuestions.Add("Wie lautet der englische Begriff zur Breitensuche?");

        //Answers for BreadthFirstSearch
        _currentAnswers.Add("O(|Kantenzahl|+|Knotenzahl|)");
        _currentAnswers.Add("Finnland");
        _currentAnswers.Add("Cotton");
        _currentAnswers.Add("Sie priorisiert weder links noch rechts, solange die Knoten in der selben Ebene liegt.");
        _currentAnswers.Add("Es werden zuerst alle Knoten betrachtet, die vom Ausgangsknoten direkt erreichbar sind.");
        _currentAnswers.Add("Wahr: Sie funktioniert bei beiden Graphentypen");
        _currentAnswers.Add("Ja - jedoch ohne Beruecksichtigung der Gewichtung.");
        _currentAnswers.Add("Nein");
        _currentAnswers.Add("Nein");
        _currentAnswers.Add("breadth first search");
    }

    /// <param name="DepthFirstSearch Questions Answers"> Creates the list of questions for DepthFirstSearch </param>
    private void depthFirstSearchQuestionsAnswers()
    {
        // Questions for DepthFirstSearch
        _currentQuestions.Add("Laufzeit von Tiefensuche?");
        _currentQuestions.Add("Was ist der tiefste Punkt der Weltmeere?");
        _currentQuestions.Add("Nimmt die Tiefensuche immer den weitesten linken oder den weitesten rechten Pfad?");
        _currentQuestions.Add("Was ist der Unterschied der Tiefensuche zur Breitensuche?");
        _currentQuestions.Add("Die Tiefensuche funktioniert nur bei gerichteten Graphen - wahr oder falsch?");
        _currentQuestions.Add("Findet die Tiefensuche immer den kuerzesten Weg?");
        _currentQuestions.Add("Ist die Gewichtung der Kanten bei der Tiefensuche wichtig?");
        _currentQuestions.Add("Sind heuristische Werte bei der Tiefensuche wichtig?");
        _currentQuestions.Add("Wie lautet der englische Begriff zur Tiefensuche?");
        _currentQuestions.Add("Wie heisst die Motte, welche die Tiefensuche bevorzugt?");

        //Answers for DepthFirstSearch
        _currentAnswers.Add("O(|Kantenzahl|+|Knotenzahl|)");
        _currentAnswers.Add("Der Marianengraben");
        _currentAnswers.Add("(Fangfrage) Sie priorisiert weder links noch rechts, solange der Pfad tiefer fuehrt.");
        _currentAnswers.Add(
            "Sie sucht einen Pfad zuerst vollstaendig in die Tiefe, bevor sie abzweigende Pfade betrachtet");
        _currentAnswers.Add("Falsch: Sie funktioniert auch bei ungerichteten Graphen");
        _currentAnswers.Add("Nein - sie kann auch einen laengeren Weg als Loesung praesentieren.");
        _currentAnswers.Add("Nein");
        _currentAnswers.Add("Nein");
        _currentAnswers.Add("depth first search");
        _currentAnswers.Add("Button");
    }
}