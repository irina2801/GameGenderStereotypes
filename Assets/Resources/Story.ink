-> PlayGameCanvas
=== PlayGameCanvas ===
# canvas: PlayGameCanvas
Play Game Button
-> 1Twins

=== 1Twins ===
# canvas: 1Twins
Bentornato, giudice! Ottimo lavoro nel risolvere <br> il tuo primo caso con Sophia e Theo!
-> 2Twins

=== 2Twins ===
# canvas: 2Twins
Non potevate sapere chi scegliere per <br>l'avventura perché non vi abbiamo <br>detto in cosa siamo bravi. È stato <br>anche ingiusto scegliere le parole <br>per descriverci.
* [OptionA] -> 3TwinschoosingA
* [OptionB] -> 3TwinschoosingB

=== 3TwinschoosingA ===
# canvas: 3TwinschoosingA
Esattamente! Siamo più di un ragazzo o di <br>una ragazza. Non tutti i ragazzi e le ragazze <br>sono uguali. Forse il gufo voleva <br>insegnarti qualcosa.
* [OptionA] -> 4TwinsAAndB
* [OptionB] -> 4TwinsAAndB

=== 3TwinschoosingB ===
# canvas: 3TwinschoosingA
Sapevi solo che siamo un ragazzo e una ragazza. Come potevi conoscerci davvero? Tutti i ragazzi e le ragazze che conosci sono esattamente uguali?

* [OptionA] -> 4TwinsAAndB
* [OptionB] -> 4TwinsAAndB

=== 4TwinsAAndB ===
# canvas: 4TwinsAAndB
Quando tiriamo a indovinare, ci perdiamo la <br>vera identità di una persona. Buona fortuna <br>per il prossimo caso!
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
Saluti, giovane giudice! Ti prego di far capire al <br>mio fratellino che il festival di Ares è la scelta <br>giusta per lui e NON il festival delle Muse.
* [OptionAAskBigBrother] -> 2DialogueChoiceABigBrother
* [OptionBAskTheLittleBrother] -> 2DialogueChoiceBLittleBrother


=== 2DialogueChoiceABigBrother === 
# canvas: 2DialogueChoiceABigBrother
//Big brother talks
L'arte e la letteratura sono cose per ragazze. <br>I ragazzi dovrebbero essere bravi negli sport o <br>nelle scienze come la matematica e la fisica. <br>Mio fratello deve imparare a combattere e a <br>giocare a calcio al festival di Ares. 
* [OptionA: Challenge] -> choiceAChallengedSubjects
* [OptionB] -> choiceBNOTChallengedSubjects

=== 2DialogueChoiceBLittleBrother ===
# canvas: 2DialogueChoiceBLittleBrother
//Little Brother Talks
Mi piacerebbe andare al festival delle Muse, <br>ma mio fratello Alexandros pensa che sia per le <br>ragazze. Pensa che la letteratura e le arti siano <br>più da ragazze, mentre lo sport, la matematica <br>e la fisica siano più da ragazzi.
* [OptionA: Challenge] -> choiceAChallengedSubjects
* [OptionB] -> choiceBNOTChallengedSubjects

=== choiceAChallengedSubjects ===
# canvas: choiceAChallengedSubjects
Congrats! You challenged Gender Stereotypes!
-> 3DialogueLittleBrother

=== choiceBNOTChallengedSubjects ===
# canvas: choiceBNOTChallengedSubjects
Congrats! You challenged Gender Stereotypes!
-> 3DialogueLittleBrother


=== 3DialogueLittleBrother ===
# canvas: 3DialogueLittleBrother
//Little brother talking about hobbies
Voglio davvero andare al festival delle Muse <br>perché mi piacciono il balletto e la poesia. <br>Alcune persone mi prendono in giro perché <br>pensano che siano hobby femminili. 
* [OptionA: Challenge] -> ReportCanvas
* [OptionB] -> ReportCanvas

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

