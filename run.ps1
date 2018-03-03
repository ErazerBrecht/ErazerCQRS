start powershell {
cd ~
    redis-server.exe
}


start powershell { 
cd Erazer.Web.ReadAPI
./startmongo.ps1
}

start powershell { 
cd Erazer.Web.WriteAPI
./start_eventstore.ps1
}

start powershell { 
cd Erazer.Web.Websockets
node server.js 
}

Start-Sleep -s 5


start powershell { 
cd Erazer.Web.ReadAPI
dotnet run 
}

Start-Sleep -s 10

start powershell { 
cd Erazer.Web.WriteAPI
dotnet run 
}


Start-Sleep -s 10

start powershell { 
cd Erazer.Web.DocumentStore
dotnet run 
}
