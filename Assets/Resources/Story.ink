-> PlayGameCanvas
=== PlayGameCanvas ===
# canvas: PlayGameCanvas
Play Game Button
-> 1Twins

=== 1Twins ===
# canvas: 1Twins
Bentornato, giudice! Ottimo lavoro nel risolvere il tuo primo caso con Sophia e Theo!
-> 2Twins

=== 2Twins ===
# canvas: 2Twins
Non potevate sapere chi scegliere per l'avventura perché non vi abbiamo detto in cosa siamo bravi. È stato anche ingiusto scegliere le parole per descriverci.
* [OptionA] -> 3TwinschoosingA
* [OptionB] -> 3TwinschoosingB

=== 3TwinschoosingA ===
# canvas: 3TwinschoosingA
The twins
* [OptionA] -> 4TwinsAAndB
* [OptionB] -> 4TwinsAAndB

=== 3TwinschoosingB ===
# canvas: 3TwinschoosingA
The twins
* [OptionA] -> 4TwinsAAndB
* [OptionB] -> 4TwinsAAndB

=== 4TwinsAAndB ===
# canvas: 4TwinsAAndB
The twins
-> PresentingBrother

=== PresentingBrother ===
# canvas: PresentingBrother
Presenting the 2 brothers
-> ScenarioBrothers

=== ScenarioBrothers ===
# canvas: ScenarioBrothers
Scenario
-> 1DialogueBigBrother

=== 1DialogueBigBrother ===
# canvas: 1DialogueBigBrother
Big Brother Talks: Greetings, jury...
* [OptionAAskBigBrother] -> 2DialogueChoiceABigBrother
* [OptionBAskTheLittleBrother] -> 2DialogueChoiceBLittleBrother


=== 2DialogueChoiceABigBrother ===
# canvas: 2DialogueChoiceABigBrother
Big Brother Talks
* [OptionA: Challenge] -> choiceAChallengedSubjects
* [OptionB] -> choiceBNOTChallengedSubjects

=== 2DialogueChoiceBLittleBrother ===
# canvas: 2DialogueChoiceBLittleBrother
Little Brother Talks
* [OptionA: Challenge] -> choiceAChallengedSubjects
* [OptionB] -> choiceBNOTChallengedSubjects

=== choiceAChallengedSubjects ===
# canvas: choiceAChallengedSubjects
Congrats! You challenged Gender Stereotypes!
-> ReportCanvas

=== choiceBNOTChallengedSubjects ===
# canvas: choiceBNOTChallengedSubjects
Congrats! You challenged Gender Stereotypes!
-> ReportCanvas

=== ReportCanvas ===
# canvas: ReportCanvas
Final Report
-> END





/*
=== Athena3Intro ===
# canvas: Athena3Intro
Your job is to give advice to the people of Athens who come to you for help. <br><br>I am very curious what you will learn from this experience. Please enter my temple, the PARTHENON, on the ACROPOLIS hill. <br><br>Goodbye!(
->ReadyOrNot
*/

