# Automatic Article Downloader and Forwarder

The purpose of this project is to make it easy to email the full text of articles from a handheld device (or any device that doesn't have good copy and paste support.)

# Setup

This project is set up to use gmail and IMap. You can use other email providers and protocols, but must modify the code. 

**Things you will need**

 - Two email addresses. 
    - One is the primary email address your messages will be from (you probably already have this).
    - Two is the email address that this app will be checking for new messages. You may need to create a new address for this.
- A [Readability API Account](https://www.readability.com/developers/api). This is free to use for personal use. It is also rate-capped, so you can't go posting thousands of messages.

**Getting Started**
Pull the source code and open the .sln file with Visual Studio. Open the ImapPoster/App.config file. You need to fill in the app settings. These are the settings.

      <appSettings>
        <add key="ReadbilityBaseUrl" value="https://readability.com/api/content/v1/parser"/>
        <add key="ReadabilityParserApi_Token" value="YOUR TOKEN"/>
        <add key="GatewayEmailAddress" value="YOUR GATEWAY ADDRESS (the address you send the urls to)"/>
        <add key="GatewayEmailPassword" value="The password for your gateway address"/>
        <add key="EmailAddress" value="YOUR REAL EMAIL ADDRESS (the address the emails will be FROM)"/>
        <add key="EmailAddressPassword" value="APP SPECIFIC PASSWORD for your gmail account"/>
        <add key="ToEmailAddress" value="THE EMAIL ADDRESS WHERE THE MESSAGES WILL BE SENT"/>
    </appSettings>

- **ReadabilityParserApi_Token**: the token you got when you signed up for readability api
- **GatewayEmailAddress**: The new email address you created. This is where you will send your messages to.
- **GatewayEmailPassword**: The password for the above.
- **EmailAddress**: The email address this app will send emails as.
- **EmailAddressPassword**: The password for the above account. If you use Two Factor Authentication you will need to create an App Specific Password for this app.
- **ToEmailAddress**: The address this app will send emails to.

# Running
Once you have configured the above, run the app and make sure it can successfully check emails. Send an email to your gateway account with a URL in it and make sure you receive the full article body at the To email address location. (for testing purposes you can make the EmailAddress and ToEmailAddress the same).

Once you verify it is working, you will want this thing to run on a schedule. I use Windows Task Scheduler to run this application every 5 minutes. It wakes up, checks for emails, if there are any, it grabs them, gets the article bodies, and forwards them on.

# Using
When you want an email fowarded with a body, you must construct your email in a certain way.

* Email Subject - the subject of the email becomes comments before the article in the resulting email.
* Email Body - the body of the email must start with a URL. If you have anything after that (such as an automatic email signature) make sure it comes after a "--" on its own line.

**Example**

An email structured like this:

    TO: FakeEmail@Gmail.com
    SUBJECT: I thought this article was really great. It really spoke to my soul.
    BODY:
    https://medium.com/@tonx/anthony-bourdains-shade-thrown-coffee-1daf701554dd#.nes4wxsli
    --
    Love, Patrick

Will become an email that looks like this:

    TO: Target@Gmail.com
    FROM: You@Gmail.com
    SUBJECT: Anthony Bourdain’s Shade Thrown Coffee
    BODY
    I thought this article was really great. It really spoke to my soul.
    
    I try to avoid getting sucked into writing hot takes, but...
    (rest of article)

