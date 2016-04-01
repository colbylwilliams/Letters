#!/bin/bash

# c0lby:

## Create New Root.plist file ##

PreparePreferenceFile

		AddNewTitleValuePreference  -k "VersionNumber" 	-d "$versionNumber ($buildNumber)" 	-t "Version"

		AddNewTitleValuePreference  -k "GitCommitHash" 	-d "$gitCommitHash" -t "Git Hash"


	AddNewPreferenceGroup	-t "Letter Drawing"

		AddNewMultiValuePreference  -k "LetterDrawDuration"		-d 3 		-t "Letter Draw Duration"
			SetMultiValuePreferenceValues  1 2 3 4 5 6 7 8 9 10
			SetMultiValuePreferenceTitles  "1 second" "2 seconds" "3 seconds" "4 seconds" "5 seconds" "6 seconds" "7 seconds" "8 seconds" "9 seconds" "10 seconds"

		AddNewMultiValuePreference  -k "LetterDrawDelay"   		-d 5 		-t "Next Letter Delay"
			SetMultiValuePreferenceValues  1 2 3 4 5 6 7 8 9 10
			SetMultiValuePreferenceTitles  "1 second" "2 seconds" "3 seconds" "4 seconds" "5 seconds" "6 seconds" "7 seconds" "8 seconds" "9 seconds" "10 seconds"

		AddNewToggleSwitchPreference 	-k "LetterDrawLoop" 	-d true 	-t "Continuously Loop"


	AddNewPreferenceGroup 	-t "Diagnostics Key"
		AddNewStringNode 	-e "FooterText" 	-v "$copyright"


	AddNewTitleValuePreference  -k "UserReferenceKey" 	-d "ANONYMOUS"  	-t ""
