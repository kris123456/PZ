﻿using SkyCrab.Common_classes.Players;

namespace SkyCrab.Connection.PresentationLayer.Messages.Menu
{
    /// <summary>
    /// <para>Sender: Client</para>
    /// <para>ID: <see cref="MessageId.EDIT_PROFILE"/></para>
    /// <para>Data type: <see cref="PlayerProfile"/> (without login, registration and lastActivity)</para>
    /// <para>Passible answers: <see cref="Ok"/>, <see cref="Error"/></para>
    /// <para>Error codes: <see cref="ErrorCode.NICK_IS_TOO_SHITTY"/>, <see cref="ErrorCode.PASSWORD_TOO_SHORT2"/>, <see cref="ErrorCode.EMAIL_OCCUPIED2"/></para>
    /// </summary>
    public sealed class EditProfile : AbstractMessage
    {

        public override MessageId Id
        {
            get { return MessageId.EDIT_PROFILE; }
        }

        internal override bool Answer
        {
            get { return false; }
        }

        internal override object Read(MessageConnection connection)
        {
            string password = connection.SyncReadData(MessageConnection.stringTranscoder);
            string nick = connection.SyncReadData(MessageConnection.stringTranscoder);
            string eMail = connection.SyncReadData(MessageConnection.stringTranscoder);
            PlayerProfile playerProfile = new PlayerProfile();
            playerProfile.password = password;
            playerProfile.nick = nick;
            playerProfile.eMail = eMail;
            return playerProfile;
        }

        public static MessageConnection.MessageInfo? SyncPostEditProfile(MessageConnection connection, PlayerProfile playerProfile, int timeout)
        {
            return SyncPost((callback, state) => AsyncPostEditProfile(connection, playerProfile, callback, state), timeout);
        }

        public static void AsyncPostEditProfile(MessageConnection connection, PlayerProfile playerProfile, MessageConnection.AnswerCallback callback, object state = null)
        {
            MessageConnection.MessageProcedure messageProc = (writingBlock) =>
            {
                connection.AsyncWriteData(MessageConnection.stringTranscoder, writingBlock, playerProfile.password);
                connection.AsyncWriteData(MessageConnection.stringTranscoder, writingBlock, playerProfile.nick);
                connection.AsyncWriteData(MessageConnection.stringTranscoder, writingBlock, playerProfile.eMail);
                connection.SetAnswerCallback(writingBlock, callback, state);
            };
            connection.PostMessage(MessageId.EDIT_PROFILE, messageProc);
        }

    }
}
