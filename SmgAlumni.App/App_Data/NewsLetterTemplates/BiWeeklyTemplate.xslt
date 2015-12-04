<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
>
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
          #backgroundTable{height:100% !important; margin:0; padding:0; width:100% !important;}

          p {
          margin: 1em 0;
          }

          table td {
          border-collapse:collapse;
          }
        </style>

        <!--<xsl:variable name="lastEvent" select="/ActivityEmail/Events/Event[last()]" />
        <xsl:variable name="projectID" select="$lastEvent/Metadata/EventMetadata[Name/text()='ProjectID']/Value" />
        <xsl:variable name="eventName" select="$lastEvent/EventName" />
        <xsl:variable name="entityType" select="$lastEvent/Metadata/EventMetadata[Name/text()='RootEntityType']/Value"/>-->

        <div style="font:16px 'Segoe UI', Arial, Helvetica, sans-serif">

          <!--<h5 style="margin:2px">
            <xsl:choose>
              <xsl:when test="$eventName = 'WorkItemChanged'">
                <xsl:value-of select="/ActivityEmail/ProjectName"/>
              </xsl:when>
              <xsl:otherwise>
                TeamPulse
              </xsl:otherwise>
            </xsl:choose>
          </h5>-->

          <hr/>

      <!--<xsl:if test="$eventName = 'WorkItemChanged'">
          
          <h4 style="margin:4px 0 16px;font-size:21px;line-height:110%;font-weight:normal;">
            <xsl:choose>
              --><!-- Use this title and link when sending "public" feedback emails --><!--
              <xsl:when test="$entityType = 'Feedback' and /ActivityEmail/UserCanAccessFeed = 'false'">
                <a style="color:#AA2B40;" href="{/ActivityEmail/FeedbackPortalUrl}Project/{$projectID}/Feedback/Details/{$lastEvent/Metadata/EventMetadata[Name/text()='RootEntityID']/Value}">
                  <xsl:value-of select="$lastEvent/Metadata/EventMetadata[Name/text()='Name']/Value"/>
                </a>
              </xsl:when>
              --><!-- Use this title when sending all other emails --><!--
              <xsl:otherwise>
                <xsl:value-of select="$entityType"/>:
                <xsl:if test="$lastEvent/Metadata/EventMetadata[Name/text()='Name']/Value">
                    <xsl:choose>
                        <xsl:when test="$lastEvent/HyperlinkUrl">
                            <a style="color:black;" >
                                <xsl:attribute name="href">
                                    <xsl:value-of select="$lastEvent/HyperlinkUrl" disable-output-escaping="yes" />
                                </xsl:attribute>
                                <xsl:value-of select="$lastEvent/Metadata/EventMetadata[Name/text()='Name']/Value"/>
                            </a>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:value-of select="$lastEvent/Metadata/EventMetadata[Name/text()='Name']/Value"/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:if>
                <xsl:if test="$entityType = 'Feedback' and $lastEvent/Metadata/EventMetadata[Name/text()='IsVisibleInPortal']/Value = 'True' and /ActivityEmail/FeedbackPortalUrl">
                  <div style="font-size:12px;color:#666;">
                    [ <a style="color:#666;" href="{/ActivityEmail/FeedbackPortalUrl}Project/{$projectID}/Feedback/Details/{$lastEvent/Metadata/EventMetadata[Name/text()='RootEntityID']/Value}">view in portal</a> ]
                  </div>
                </xsl:if>
              </xsl:otherwise>
            </xsl:choose>
          </h4>

          --><!--<xsl:call-template name="work-item-header">
            <xsl:with-param name="firstEvent" select="$lastEvent" />
            <xsl:with-param name="userCanAccessFeed" select="/ActivityEmail/UserCanAccessFeed" />
          </xsl:call-template>--><!--
          
      </xsl:if>-->


      <xsl:for-each select="/ActivityEmail/Events/Event">
            <div style="margin-top:20px;color:#666;font-size:14px;">
              <xsl:value-of select="AuthorDisplayName"/>
              <xsl:if test="EventName = 'PersonalMessage'">
                <span style="color:black;"> shared:</span>
              </xsl:if>
            </div>
            <div>
              <xsl:value-of select="ShortContent" disable-output-escaping="yes"/>
            </div>
      </xsl:for-each>
          
          <HR style="margin-top:20px;"/>

          <xsl:if test="$entityType = 'Feedback' and /ActivityEmail/UserCanAccessFeed = 'false' and /ActivityEmail/FeedbackPortalUrl">
            <div style="color:#666;font-size:12px;">
              You are receiving this email because you have email notifications enabled for items you follow and you are following this item. If you no longer want to get emails like this you can <a href="{/ActivityEmail/FeedbackPortalUrl}Project/{$projectID}/Account/Unfollow/{$lastEvent/Metadata/EventMetadata[Name/text()='RootEntityID']/Value}">unfollow this item</a> or edit your user account settings to <a href="{/ActivityEmail/FeedbackPortalUrl}Project/{$projectID}/Account/EditProfile">disable all emails</a> on items you follow.
            </div>
          </xsl:if>
            
          <xsl:if test="/ActivityEmail/UserCanAccessFeed = 'true'">
            <div style="color:#666;font-size:12px;">
              You are receiving this email because it matches an email notification rule you have enabled in TeamPulse.&#160;&#160;If you no longer want to get emails like this, please adjust your <a style="color: #333" href="{/ActivityEmail/TeamPulseHostUrl}Admin/Profile/Notifications">notification rules</a>.
              <xsl:if test="$entityType = 'Feedback' and /ActivityEmail/FeedbackPortalUrl">
                <span>
                  &#160;If you are following this item, you may want to <a style="color: #333" href="{/ActivityEmail/FeedbackPortalUrl}Project/{$projectID}/Account/Unfollow/{$lastEvent/Metadata/EventMetadata[Name/text()='RootEntityID']/Value}">unfollow it</a>.
                </span>
              </xsl:if>
            </div>
          </xsl:if>

          <span style="color:#666;font-size:12px;">
            Powered by <a href="http://www.telerik.com/teampulse">TeamPulse</a>.
          </span>
        </div>
      </body>
    </html>
  </xsl:template>
  
  
  <!--<xsl:template name="work-item-header">
    <xsl:param name="firstEvent" />
    <xsl:param name="userCanAccessFeed" />
    <xsl:variable name="entityType" select="$firstEvent/Metadata/EventMetadata[Name/text()='RootEntityType']/Value" />

    <table border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td valign="top" style="padding-right:50px;font-size:12px;color:#999;text-align:left;vertical-align:top;">
          <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='FeedbackType']/Value and $firstEvent/Metadata/EventMetadata[Name/text()='CreatedBy']/Value">
            <div>
              <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='FeedbackType']/Value"/> by: <span style="color:black;">
                <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='CreatedBy']/Value"/>
              </span>
            </div>
          </xsl:if>
          <div>
            Status:
            <span style="color:black;">
              <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='Status']/Value"/>
            </span>
          </div>
          <xsl:if test="$userCanAccessFeed = 'true'">
            <div>
              Area:
              <span style="color:black;">
                <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='AreaPath']/Value"/>
              </span>
            </div>
            <div>
              Iteration:
              <span style="color:black;">
                <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='NewIterationName']/Value"/>
              </span>
            </div>
          </xsl:if>
          <xsl:if test="$userCanAccessFeed = 'false'">
            <xsl:if test="$entityType = 'Feedback' and $firstEvent/Metadata/EventMetadata[Name/text()='PortalShowAreas']/Value = 'True'">
              <div>
                Category:
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='PortalCategory']/Value"/>
                </span>
              </div>
            </xsl:if>
            <xsl:if test="$entityType = 'Feedback' and $firstEvent/Metadata/EventMetadata[Name/text()='PortalShowIterations']/Value = 'True'">
              <div>
                Scheduled for:
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='PortalScheduledFor']/Value"/>
                </span>
              </div>
            </xsl:if>
          </xsl:if>
          <xsl:if test="$entityType != 'Feedback'">
            <div>
              Estimate:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='Estimate']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='Estimate']/Value"/>
                </span>
              </xsl:if>
            </div>
            <div>
              Assigned To:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='AssignedTo']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='AssignedTo']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>
          <xsl:if test="$entityType = 'Feedback' and $firstEvent/Metadata/EventMetadata[Name/text()='IsVisibleInPortal']/Value = 'True'">
            <div>
              <xsl:if test="$userCanAccessFeed = 'true'">
                <span>Portal</span>
              </xsl:if>
              Comments:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='PortalCommentCount']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='PortalCommentCount']/Value"/>
                </span>
              </xsl:if>  
            </div>
            <div>
              <xsl:if test="$userCanAccessFeed = 'true'">
                <span>Portal</span>
              </xsl:if>
              Attachments:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='PortalAttachmentCount']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='PortalAttachmentCount']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>
        </td>
        <td valign="top" style="font-size:12px;color:#999;text-align:left;vertical-align:top;">
          <xsl:if test="$userCanAccessFeed = 'true'">
            <div>
              ID:
              <span style="color:black;">
                <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='RootEntityID']/Value"/>
              </span>
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='RootTfsID']/Value">
                &#160;TFS ID:&#160;
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='RootTfsID']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>
          
          <xsl:if test="$entityType = 'Story'">
            <div>
              Priority Class:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='PriorityClass']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='PriorityClass']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>

          <xsl:if test="$entityType = 'Bug' or $entityType = 'Issue' or $entityType = 'Risk'">
            <div>
              Severity:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='Severity']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='Severity']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>

          <xsl:if test="$entityType != 'Feedback'">
            <div>
              Team:
              <xsl:if test="$firstEvent/Metadata/EventMetadata[Name/text()='AssignedToTeam']/Value">
                <span style="color:black;">
                  <xsl:value-of select="$firstEvent/Metadata/EventMetadata[Name/text()='AssignedToTeam']/Value"/>
                </span>
              </xsl:if>
            </div>
          </xsl:if>
        </td>
      </tr>
    </table>
    
    <div style="font-size:12px; color:#999;">
      Last update:
      <span style="color:black;">
        <xsl:value-of select="/ActivityEmail/LastModifiedDateTimeLocal"/> by <xsl:value-of select="$firstEvent/AuthorDisplayName"/>
      </span>
    </div>
  </xsl:template>-->
</xsl:stylesheet>