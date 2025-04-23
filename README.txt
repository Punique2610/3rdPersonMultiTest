How to test:
- There is a Build.rar file that contains a build version of the game. Take it to two different machines, extract it, then run.
- Or you can run the game on Unity Editor and the build on different machine.
- Or you can run the game on Unity Editor and the build on the same machine(not recommended).
- From one machine: Main Menu => Play => Lobby => Create A Random Room.
- After you created a room on one client, on the other client: Main Menu => Play => Lobby => Join the room that was just created in the room list.
- IMPORTANT NOTE: Sometimes, when the latter client joins the lobby, you will not see the former created room. That's because sometimes Photon fails to detect the region.
This is on Photon side. When you encounter this, just go back to the menu and join the lobby again, repeat until you see the room. Or you can restart the game.

Tools used:

- Third Person Controller of Invector: for 3rd-Person basic movement.
- 3D Modern Menu UI of SlimUI: for main menu UI.
- Photon.

I design the system based on Clean Architechture of Robert C. Martin.
When using others' assets/libraries, sometimes they were not designed with the same principles as our codebase.
So I tried to integrate the assets/libraries in a way that made use of their strength as much as possible, while also maintaining the clean architecture.

Below are all the scripts I wrote and their layer in the architecture, and the dependency direction.
With clean architecture, they are loosely coupled, clear to understand, and great for scalability.


                      ┌──────────────────────────────────────────────────────┐
                      │          Frameworks/Engine/Network Layer             │
                      │                                                      │
                      │ ┌───────── Player/Engine-related     ────────────┐   │
                      │ │  • PlayerNetworkCompositeRoot                  │   │
                      │ │  • PlayerNetworkedProperties                   │   │
                      │ │  • pThirdPersonAnimator                        │   │
                      │ │  • pThirdPersonMovementAnimating               │   │
                      │ │  • pThirdPersonMovementInputFusionAdapter      │   │
                      │ │  • pThirdPersonMovementInputProvider           │   │
                      │ │  • pThirdPersonCamera                          │   │
                      │ │  • pThirdPersonCameraFusionAdapter             │   │
                      │ └────────────────────────────────────────────────┘   │
                      │                                                      │
                      │ ┌───────── UI/Management     ────────────────────┐   │
                      │ │  • UIManager                                   │   │
                      │ │  • MainMenuUI                                  │   │
                      │ │  • LobbyUI                                     │   │
                      │ │  • RoomUI                                      │   │
                      │ │  • RoomSessionEntryUI                          │   │
                      │ │  • PlayerHeadUI                                │   │
                      │ │  • GameManager                                 │   │
                      │ │  • LobbyManager                                │   │
                      │ │  • NetworkRoomManager                          │   │
                      │ │  • RoomManager                                 │   │
                      │ │  • pSceneManager                               │   │
                      │ └────────────────────────────────────────────────┘   │
                      └──────────────────────────────────────────────────────┘
                                         │
                                         ▼
                      ┌──────────────────────────────────────────────────────┐
                      │          Adapters/Controllers Layer                  │
                      │                                                      │
                      │     • pThirdPersonController                         │
                      │     • PlayerNetworkedPropertiesUpdator               │
                      └──────────────────────────────────────────────────────┘
                                         │
                                         ▼
                      ┌──────────────────────────────────────────────────────┐
                      │                  Use Cases Layer                     │
                      │                                                      │
                      │     • pThirdPersonMovement                           │
                      │     • pThirdPersonMovementHandler                    │
                      └──────────────────────────────────────────────────────┘
                                         │
                                         ▼
                      ┌──────────────────────────────────────────────────────┐
                      │                 Entities Layer                       │
                      │                                                      │
                      │     • PlayerBaseData                                 │
                      │     • PlayerBaseDataServer                           │
                      │     • pThirdPersonMotor                              │
                      └──────────────────────────────────────────────────────┘


 


