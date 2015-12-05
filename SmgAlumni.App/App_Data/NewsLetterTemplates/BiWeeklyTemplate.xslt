<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- indicates what our output type is going to be -->
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <body>
        <style type="text/css">

          /* Client-specific Styles */
          #outlook a{padding:0;} /* Force Outlook to provide a "view in browser" button. */
          body{width:100% !important;} .ReadMsgBody{width:100%;} .ExternalClass{width:100%;} /* Force Hotmail to display emails at full width */
          body{-webkit-text-size-adjust:none; -ms-text-size-adjust:none;} /* Prevent Webkit and Windows Mobile platforms from changing default font sizes. */

          /* Reset Styles */
          body{margin:0; padding:0;font-family: 'Segoe UI', Arial, Helvetica, sans-serif;}
        </style>
          <hr/>

      <h3>Благотворителни каузи</h3>
      <xsl:for-each select="/BiMonthlyNewsLetterDto/Causes">
        <div> 
            <div>
              <xsl:value-of select="CreatedOn"/>
            </div>
            <div>
              <xsl:value-of select="CreatedBy/UserName"/>
            </div>
            <div>
              <xsl:value-of select="HtmlBody" disable-output-escaping="yes"/>
            </div>
          </div>
      </xsl:for-each>
          
      <hr style="margin-top:20px;"/>

          <h3>Новини</h3>
      <xsl:for-each select="/BiMonthlyNewsLetterDto/News">
        <div> 
            <div>
              <xsl:value-of select="CreatedOn"/>
            </div>
            <div>
              <xsl:value-of select="CreatedBy/UserName"/>
            </div>
            <div>
              <xsl:value-of select="HtmlBody" disable-output-escaping="yes"/>
            </div>
          </div>
      </xsl:for-each>
          
      <hr style="margin-top:20px;"/>

          <h3>Обяви</h3>
     <xsl:for-each select="/BiMonthlyNewsLetterDto/Listings">
        <div> 
            <div>
              <xsl:value-of select="CreatedOn"/>
            </div>
            <div>
              <xsl:value-of select="CreatedBy/UserName"/>
            </div>
            <div>
              <xsl:value-of select="HtmlBody" disable-output-escaping="yes"/>
            </div>
          </div>
      </xsl:for-each>
          
      <hr style="margin-top:20px;"/>

          <h3>Нови потребители</h3>
        <xsl:for-each select="/BiMonthlyNewsLetterDto/AddedUsers">
          <div>
            <span>
              <xsl:value-of select="HtmlBody" disable-output-escaping="yes"/>
          </span>  
          </div>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>