public abstract class IAbility
{
    public Cooldown Cooldown { get; private set; }

    public abstract void Command();

    public abstract void ClientUse();

    public abstract void ServerUse();
}