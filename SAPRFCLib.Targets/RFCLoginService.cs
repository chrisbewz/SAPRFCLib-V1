using System;
using System.ComponentModel;

namespace SAPRFCLib.Targets;

public class RFCLoginServices
{
    public delegate void LoginEventHandler(object source, LoginInformationArgs args);

    public event LoginEventHandler LoginCredentialsChanged;
    public event EventHandler LoginStateChanged;
    public event LoginEventHandler LoginRequested;

    public virtual void OnLoginCredentialsChanged(Credentials data)
    {
        LoginCredentialsChanged?.Invoke(this, new LoginInformationArgs(data));
    }

    public virtual void OnLoginRequested(Credentials data)
    {
        LoginRequested?.Invoke(this, new LoginInformationArgs(data));
    }
    
    public virtual void OnLoginStateChanged(EventArgs e)
    {
        LoginStateChanged?.Invoke(this, EventArgs.Empty);
    }

}
public class LoginInformationArgs : EventArgs
{
    [DefaultValue((object)null)]
    public string UserName { get; set; }

    [DefaultValue((object)null)]
    public string Password { get; set; }
    
    public bool LoginState { get; set; }

    public LoginInformationArgs(Credentials information)
    {
        UserName = information.Name;
        Password = information.Password;
    }
}

public class Credentials : ICredentiable
{
    private string _name;
    private string _password;

    public Credentials()
    {
        _name = null;
        _password = null;
    }

    public Credentials( string user,string passphrase)
    {
        _name = user;
        _password = passphrase;
    }
    public string Password { get; set; }
    public string Name { get; set; }

    public void SetCredentials()
    {

    }
}

public interface ICredentiable
{
    public string Name { get; set; }
    public string Password { get; set; }
}