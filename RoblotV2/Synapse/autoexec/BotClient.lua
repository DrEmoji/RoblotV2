game.Loaded:Wait()

-- Variables
local uis = game:GetService("UserInputService")
local WebSocket = syn.websocket.connect("ws://localhost:5000/Boblox")
local Player = game:GetService("Players").LocalPlayer
local ReplicatedStorage = game:GetService("ReplicatedStorage")
local botnum = 0
local PosOffset = Vector3.new(0,0,0)
local IsBot = true
_G.OrbitConnection = nil
_G.MimicConnection = nil
_G.FollowConnection = nil

-- Commands
function RunCommand(Msg)
    local success, errorMessage = pcall(function()
        if string.find(Msg,"/") then
            CMDInfo = string.split(Msg,"/")
            CMD = CMDInfo[1]
            detail = CMDInfo[2]
        else
            CMD = Msg
            detail = ""
        end
        print(CMD)
        if CMD == "sendid" and Player.Name == detail then
            botnum = tonumber(CMDInfo[3])
            if CMDInfo[4] == "False" then
                IsBot = false
            else
                UserSettings():GetService("UserGameSettings").MasterVolume = 0
            end
        elseif CMD == "jump" then
            print("Action Found: Jump")
            Player.Character.Humanoid.Jump = true
        elseif CMD == "spam" then
            print("Action Found: Spam")
            detail = string.gsub(detail,"$"," ")
            for count = 1,10,1 do
                ReplicatedStorage.DefaultChatSystemChatEvents.SayMessageRequest:FireServer(detail,"All")
                wait(2.5)
            end
        elseif CMD == "mimic" then
            print("Action Found: Mimic")
            if game.Players:FindFirstChild(detail) then
                localplayer = game.Players.LocalPlayer.Character
                target = game.Players[detail].Character.PrimaryPart
                localplayer.PrimaryPart.Anchored = true
                
                _G.MimicConnection = game:GetService('RunService').Stepped:connect(function()
                    localplayer:SetPrimaryPartCFrame((target.CFrame + target.CFrame.LookVector) + PosOffset)
                end)
            end
        elseif CMD == "AddOffset" then
            if detail == "X" then
                PosOffset = PosOffset + Vector3.new(0.2,0,0)
            elseif detail == "Y" then
                PosOffset = PosOffset + Vector3.new(0,0.2,0)
            elseif detail == "Z" then
                PosOffset = PosOffset + Vector3.new(0,0,0.2)
            end
        elseif CMD == "RemoveOffset" then
            if detail == "X" then
                PosOffset = PosOffset + Vector3.new(-0.2,0,0)
            elseif detail == "Y" then
                PosOffset = PosOffset + Vector3.new(0,-0.2,0)
            elseif detail == "Z" then
                PosOffset = PosOffset + Vector3.new(0,0,-0.2)
            end
        elseif CMD == "runcode" then
            loadstring(detail)()
        elseif CMD == "printnum" then
            WebSocket:Send("Log/"..Player.Name.." ID is "..botnum)
        elseif CMD == "remotespam" then
            game:GetService('RunService').RenderStepped:Connect(function()
                for i,v in pairs(game:GetDescendants()) do
                    if v:IsA("RemoteEvent") then
                        v:FireServer()
                    elseif v:IsA("ClickDetector") then
                        fireclickdetector(v)
                    elseif v:IsA("Sound") then
                        v:Play()
                    elseif v:IsA("RemoteFunction") then
                        v:InvokeServer()
                    end
                end 
            end)
        elseif CMD == "tpto" then
            croq = detail:split(":")
            position = Vector3.new(croq[1],croq[2],croq[3])
            Player.Character:SetPrimaryPartCFrame(CFrame.new(position))
        elseif action == "walkto" then
            croq = detail:split(":")
            position = Vector3.new(croq[1],croq[2],croq[3])
            local playercharacter = game.Players.LocalPlayer.Character
            local Humanoid = playercharacter.Humanoid
            local Root = playercharacter.HumanoidRootPart
            local path = PathfindingService:CreatePath()
            path:ComputeAsync(Root.Position, position)
            local waypoints = path:GetWaypoints()
            for i, waypoint in ipairs(waypoints) do
                Humanoid:MoveTo(waypoint.Position)
                if waypoint.Action == Enum.PathWaypointAction.Jump then
                    Humanoid.Jump = true
                end
                Humanoid.MoveToFinished:Wait()
            end
        elseif CMD == "printplayers" then
            for i,v in pairs(game.Players:GetChildren()) do
                WebSocket:Send("Log/"..v.Name)
            end
        elseif CMD == "joinworld" then
            print("Action Found: JoinWorld")
            local place = detail:split(":")
            WebSocket:Send("ResetNum")
            game:GetService("TeleportService"):TeleportToPlaceInstance(place[1], place[2], Player)
        elseif CMD == "follow" then
            print("Action Found: Follow")
            if game.Players:FindFirstChild(detail) then
                localplayer = game.Players.LocalPlayer.Character
                target = game.Players[detail].Character.PrimaryPart
                _G.FollowConnection = game:GetService('RunService').Stepped:connect(function()
                    local position = target.CFrame + target.CFrame.LookVector * (-4 * botnum)
                    game.Players.LocalPlayer.Character.Humanoid:MoveTo(position.p)
                end)
            end
        elseif CMD == "orbit" then
            print("Action Found: Orbit")
            if game.Players:FindFirstChild(detail) then
                wait(0.5 * (botnum - 1))
                local center = game.Players[detail].Character.PrimaryPart
                local planet = game.Players.LocalPlayer.Character.PrimaryPart
                local ORBIT_TIME = 2
                local RADIUS = 5 -- how far the orbit is
                local ECLIPSE = 1 -- ranges from 0 to 1, perfect circle if 1
                local ROTATION = CFrame.Angles(0,0,0) --rotate which direction to rotate around


                local sin, cos = math.sin, math.cos
                local ROTSPEED = math.pi*2/ORBIT_TIME
                ECLIPSE = ECLIPSE * RADIUS
                local runservice = game:GetService('RunService')

                -- There are many ways to run this loop, you could do while true do if you want, if so use this: rot = rot + wait() * ROTSPEED 
                local rot = 0
                _G.OrbitConnection = game:GetService('RunService').Stepped:connect(function(t, dt)
                    rot = rot + dt * ROTSPEED
                    planet.CFrame = ROTATION * CFrame.new(sin(rot)*ECLIPSE, 0, cos(rot)*RADIUS) + center.Position
                end)
            end
        elseif CMD == "stoporbit" then
            print("Stopped Orbit")
            if _G.OrbitConnection ~= nil then
                _G.OrbitConnection:Disconnect()
                _G.OrbitConnection = nil
            end
        elseif CMD == "stopmimic" then
            print("Stopped Mimic")
            if _G.MimicConnection ~= nil then
                Player.Character.PrimaryPart.Anchored = false
                _G.MimicConnection:Disconnect()
                _G.MimicConnection = nil
            end
        elseif CMD == "stopfollow" then
            print("Stopped Follow")
            if _G.FollowConnection ~= nil then
                _G.FollowConnection:Disconnect()
                _G.FollowConnection = nil
            end
        end
    end)
    if success == false then
        print(errorMessage)
    end
end

-- Recieves Message From Socket
WebSocket.OnMessage:Connect(function(Msg)
    if IsBot then
        print("Recieved Command: ".. Msg)
        RunCommand(Msg)
    end
end)

-- Tells Socket Its Connected
WebSocket:Send("Auth/"..Player.Name.."/"..Player.UserId)

-- Loop On Another Thread to Keep Connection
coroutine.wrap(function()
    while wait(1) do end
end)()