#!/bin/bash

readonly K_APPLICATION='0x700000065'
readonly K_CAPS_LOCK='0x700000039'
readonly K_LANG1='0x700000090'
readonly K_LANG2='0x700000091'

hidutil property --set "{\"UserKeyMapping\":[
{\"HIDKeyboardModifierMappingSrc\":$K_APPLICATION,\"HIDKeyboardModifierMappingDst\":$K_LANG1},
{\"HIDKeyboardModifierMappingSrc\":$K_CAPS_LOCK,\"HIDKeyboardModifierMappingDst\":$K_LANG2}
]}"

# clear mapping.
# hidutil property --set '{"UserKeyMapping":[]}'
