using System;
using System.Text;
using Steamworks;
using UnityEngine;

public class SteamworksMessagingAPI
{
    private readonly MessageRegistry _messageRegistry;
    private readonly Callback<LobbyChatMsg_t> _lobbyChatMsg;
    private readonly CSteamID _lobbyId;

    public SteamworksMessagingAPI(ulong lobbyId, MessageRegistry messageRegistry)
    {
        if (!SteamManager.Initialized) throw new Exception("Steam is not open.");

        _messageRegistry = messageRegistry;
        _lobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMessage);
        _lobbyId = new CSteamID(lobbyId);
    }

    public void Send(SteamMessage message)
    {
        string serializedMessage = JsonUtility.ToJson(message);
        byte[] bytes = Encoding.ASCII.GetBytes(serializedMessage);

        SteamMatchmaking.SendLobbyChatMsg(_lobbyId, bytes, bytes.Length + 1);
    }

    private void OnLobbyChatMessage(LobbyChatMsg_t callback)
    {
        int messageId = (int)callback.m_iChatID;
        byte[] buffer = new byte[Config.Current.MessageChunkSize];
        int length = buffer.Length;

        int i = SteamMatchmaking.GetLobbyChatEntry(_lobbyId, messageId, out CSteamID sender, buffer, length,
            out EChatEntryType type);

        if (type != EChatEntryType.k_EChatEntryTypeChatMsg) return;

        LobbyPlayerInfo playerInfo = SteamworksHelper.GetPlayerInfo(sender.m_SteamID);

        string message = Encoding.ASCII.GetString(buffer, 0, i - 1);

        ConsoleLogger.Steamworks($"Received {message} from {playerInfo.Name}");

        ServerMessageBase messageBase = JsonUtility.FromJson<ServerMessageBase>(message);
        if (messageBase?.action == null) return;

        Type messageType = Type.GetType(messageBase.action);
        object obj = JsonUtility.FromJson(message, messageType);
        _messageRegistry.Invoke(obj, messageType);
    }
}