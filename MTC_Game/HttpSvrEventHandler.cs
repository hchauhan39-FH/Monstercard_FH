﻿using System;


namespace MTC_Game
{
    /// <summary>This delegate is used for <see cref="HttpSvr"/> events.</summary>
    /// <param name="sender">Sending object.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void HttpSvrEventHandler(object sender, HttpSvrEventArgs e); // delegates sind sowas wie pointers

}
