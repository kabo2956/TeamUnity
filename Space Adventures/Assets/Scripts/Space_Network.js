var gameName:String = "Space Adventures";

private var refreshing:boolean;
private var hostData:HostData[];

var btnX:float;
var btnY:float;
var btnM:float;
var btnH:float;
function Start(){
   btnX = Screen.width*.05;
   btnY = Screen.width*.05;
   btnM = Screen.width*.2;
   btnH = Screen.width*.1;
}

function startServer(){
    Network.InitializeServer(32, 25001, !Network.HavePublicAddress);
    MasterServer.RegisterHost(gameName, "SpaceAdventrues","This is a game for class");
}

function OnServerInitialized(){
    Debug.Log("Server intitialized");
}

function refreshHostList(){
    MasterServer.RequestHostList(gameName);
    refreshing = true;
}

function Update(){
    if(refreshing){
        if(MasterServer.PollHostList().Length > 0){
            refreshing = false;
            Debug.Log(MasterServer.PollHostList().Length);
            hostData = MasterServer.PollHostList();
        }
    }
}

function OnMasterServerEvent(mse:MasterServerEvent){
    if(mse == MasterServerEvent.RegistrationSucceeded){
        Debug.Log("Registered Server");
    }
}


    function OnGUI(){
        if(!Network.isClient && !Network.isServer){
        if(GUI.Button(Rect(btnX, btnY, btnM, btnH), "Start Server")){
            Debug.Log("Starting Server");
            startServer();
        }
        if(GUI.Button(Rect(btnX, btnY*1.1 + btnH, btnM, btnH), "Refresh Server")){
            Debug.Log("Refresh Host");
            refreshHostList();
        }
        if(hostData){
            for(var i:int = 0; i<hostData.length; i++){
                   if(GUI.Button(Rect(btnX*1.5 + btnM, btnY*1.1 + (btnH*i), btnM*3, btnH*0.5), hostData[i].gameName)){
                   Network.Connect(hostData[i]);
                }
            }
        }   
    }
}