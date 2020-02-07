#!/bin/bash

readonly K_APPLICATION='0x700000065'
readonly K_CAPS_LOCK='0x700000039'
readonly K_LANG1='0x700000090'
readonly K_LANG2='0x700000091'

hidutil property --set "{\"UserKeyMapping\":[
{\"HIDKeyboardModifierMappingSrc\":$K_APPLICATION,\"HIDKeyboardModifierMappingDst\":$K_LANG1},
{\"HIDKeyboardModifierMappingSrc\":$K_CAPS_LOCK,\"HIDKeyboardModifierMappingDst\":$K_LANG2}
]}"

exit 0
# ----------------
# documents

## clear mapping.
hidutil property --set '{"UserKeyMapping":[]}'

## Load LaunchAgents
launchctl load com.example.UserKeyMapping.plist
launchctl list

## LaunchAgents plist template
: << LaunchAgentsPlistTemplate
<?xml version="1.0" encoding="UTF-8"?>
<plist>
  <dict>
    <key>Label</key>
    <string>com.example.UserKeyMapping</string>

    <key>ProgramArguments</key>
    <array>
        <string>/bin/bash</string>
        <string>/path/to/UserKeyMapping.sh</string>
    </array>

    <key>RunAtLoad</key>
    <true />
  </dict>
</plist>
LaunchAgentsPlistTemplate
