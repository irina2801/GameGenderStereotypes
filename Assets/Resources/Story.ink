-> PlayGameCanvas
=== PlayGameCanvas ===
# canvas: PlayGameCanvas
Play Game Button
-> choiceASchoolSubjects 

=== choiceASchoolSubjects ===
# canvas: choiceASchoolSubjects
Have you ever heard of a boy who likes art/literature or a girl who is good at sports/math?

-> Athena2Intro

=== Athena2Intro ===
# canvas: Athena2Intro
Today, I’m on vacation. <br>I’ve chosen you to be the judge in my place!
-> Athena3Intro 

=== Athena3Intro ===
# canvas: Athena3Intro
Your job is to give advice to the people of Athens who come to you for help. <br><br>I am very curious what you will learn from this experience. Please enter my temple, the PARTHENON, on the ACROPOLIS hill. <br><br>Goodbye!
->ReadyOrNot

=== ReadyOrNot ===
# canvas: ReadyOrNot
* [YES] -> AMakingAChoice
* [NO] -> BWhyNotReady

=== AMakingAChoice ===
# canvas: AMakingAChoice
AMakingAChoice
-> END

=== BWhyNotReady ===
# canvas: BWhyNotReady
Why aren't you ready to choose yet?
-> MoreInfo1

=== MoreInfo1 ===
# canvas: MoreInfo1
* [A) GoBackAndChoose] -> END //NOT READY YET
* [B) AskMoreInfo] -> AskMoreInfo_Sophia

=== AskMoreInfo_Sophia ===
# canvas: AskMoreInfo_Sophia
-> END

