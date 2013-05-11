TrelloLog
=========

C# component for logging information, warning and exceptions in Trello. 

##Getting started
To sign up for Trello visit http://www.Trello.com

To generate an API key visit https://trello.com/docs/

###App config
<b>Be sure to edit the app config in the unit test project to set your application key and token.</b>

    <add key="Trello-ApplicationKey" value=""/>
    <!--To obtain a token set your application key then run the GetAuthURL unit test, navigate to the url and allow your app access
    once you've done this copy the token and use it here.-->
    <add key="Trello-AuthToken" value=""/>

    <!--Name this whatever you'd like this will be the name of the board logs will appear on.-->
    <add key="Trello-LogBoardName" value="Application Logs"/>
    
    <!--If left empty no organiztion will be used.-->
    <add key="Trello-Organization" value=""/>
  
  
  
  ###Thanks for checking this out
  ####-steve
