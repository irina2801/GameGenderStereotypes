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
# canvas: 3TwinschoosingB
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
-> 3DialogueLittleBrotherWeirdOrNot

=== choiceBNOTChallengedSubjects ===
# canvas: choiceBNOTChallengedSubjects
Congrats! You challenged Gender Stereotypes!
-> 3DialogueLittleBrotherWeirdOrNot


=== 3DialogueLittleBrotherWeirdOrNot ===
# canvas: 3DialogueLittleBrotherWeirdOrNot
//Little brother talking about hobbies
Pensa che sia un po' strano che a un ragazzo <br>piaccia il balletto o la poesia, giovane giudice?
* [OptionA: It is weird] -> 4DialogueLittleBrotherchoosingA
* [OptionB: It is not] -> 4DialogueLittleBrotherchoosingB


=== 4DialogueLittleBrotherchoosingA ===
# canvas: 4DialogueLittleBrotherchoosingA
//Little brother talking about hobbies
Perché pensi che sia un po' strano che a un <br>ragazzo piacciano il balletto e la poesia?
-> 5DialogueLittleBrotherHobbiesOnlyForGirlsOrBoys

=== 4DialogueLittleBrotherchoosingB ===
# canvas: 4DialogueLittleBrotherchoosingB
//Little brother talking about hobbies
Perché pensi che NON sia strano che a un <br>ragazzo piacciano il balletto e la poesia?
-> 5DialogueLittleBrotherHobbiesOnlyForGirlsOrBoys

=== 5DialogueLittleBrotherHobbiesOnlyForGirlsOrBoys ===
# canvas: 5DialogueLittleBrotherHobbiesOnlyForGirlsOrBoys
//Little brother talking about hobbies
Voglio andare al festival delle Muse perché mi <br>piacciono il balletto e la poesia. Mio fratello <br>pensa che questi hobby siano per le ragazze. <br><br>Alcuni hobby sono solo per le ragazze o anche per i ragazzi? 
* [OptionA: Not Challenging] -> ReflectionTimeHobbieschoosingA
* [OptionB: Hobbies are for everybody] -> ReflectionTimeHobbieschoosingB

=== ReflectionTimeHobbieschoosingA ===
# canvas: ReflectionTimeHobbieschoosingA
questions reflection time A
-> 6DialogueBigBrotherMOVIES

=== ReflectionTimeHobbieschoosingB ===
# canvas: ReflectionTimeHobbieschoosingB
questions reflection time B
-> 6DialogueBigBrotherMOVIES


=== 6DialogueBigBrotherMOVIES ===
# canvas: 6DialogueBigBrotherMOVIES
//I've learned from movies
Ho imparato a essere un uomo dai FILM. <br>Un ragazzo deve diventare più forte. <br>La realtà è che a nessuna ragazza <br>piacerebbe un ragazzo a cui piace la danza classica. 
-> ReflectionTimeMEDIA

=== ReflectionTimeMEDIA ===
# canvas: ReflectionTimeMEDIA
MEDIA 1
-> ReflectionTimeMEDIA2

=== ReflectionTimeMEDIA2 ===
# canvas: ReflectionTimeMEDIA2
MEDIA 2
-> Muses

=== Muses ===
# canvas: Muses
Muses
-> CHOOSING_Muses_OR_Ares

/*




CHOOSING






*/
=== CHOOSING_Muses_OR_Ares ===
# canvas: CHOOSING_Muses_OR_Ares
MAKE A CHOICE: Ares or Muses Festival
* [OptionA: 9 Muses festival] -> CHOICE_A_Muses_LittleBrother
* [OptionB: Ares festival] -> choiceBARESFestival


=== CHOICE_A_Muses_LittleBrother ===
# canvas: CHOICE_A_Muses_LittleBrother
Caro giudice, grazie. <br>Il tuo consiglio mi dà forza! <br>Anche se mio fratello la pensa <br>diversamente, seguirò il mio cuore e farò <br>ciò che amo al festival delle Muse.
-> ReflectionChoice

=== choiceBARESFestival ===
# canvas: choiceBARESFestival
Caro giudice, grazie. <br>Il tuo consiglio mi dà forza! <br><br>Anche se mio fratello la pensa <br>diversamente, seguirò il mio cuore e farò <br>ciò che amo al festival delle Muse.
-> ReflectionChoice



=== ReflectionChoice ===
# canvas: ReflectionChoice
Caro giudice, grazie per il tuo consiglio. <br><br>Ma seguirò il mio cuore e andrò al festival <br>delle Muse.<br>Voglio sfidare gli STEREOTIPI DI <br>GENERE in cui crede mio fratello.
-> Owl_LetsSee_WhatHappened





=== Owl_LetsSee_WhatHappened ===
# canvas: Owl_LetsSee_WhatHappened
Owl: The little boy challenged a gender stereotype and decided to go to the Muses festival no matter what others think. Let's see what happened...
-> Outcome

=== Outcome ===
# canvas: Outcome
Outcome from Muses festival: Dimitris is happy
-> Reflection_Outcome

=== Reflection_Outcome ===
# canvas: Reflection_Outcome
Reflection based on outcome: Challenging gender stereotypes
-> Athena1

=== Athena1 ===
# canvas: Athena1
//Athena comes back
Ciao! Sono tornatoa dalle vacanze. <br>Non è stato facile, ma ce l'hai fatta!
-> Athena2

=== Athena2 ===
# canvas: Athena2
Io e i messaggeri degli dei, Iris ed Ermes, siamo <br>curiosi di sapere cosa hai imparato come giudice. <br>Invieranno il tuo messaggio al mondo. 
-> Letter

=== Letter ===
# canvas: Letter
Letter
-> GOODBYE

=== GOODBYE ===
# canvas: GOODBYE
GOODBYE
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

