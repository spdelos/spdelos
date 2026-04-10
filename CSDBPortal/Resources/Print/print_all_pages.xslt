<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
    xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://www.w3.org/TR/xhtml1/strict"
    xmlns:mycode="com.delos.model.Processing" version="1.0"
    xmlns:pdf="http://xmlgraphics.apache.org/fop/extensions/pdf"
    xmlns:fox="http://xmlgraphics.apache.org/fop/extensions">
    <!-- USE ONLY FOR JSS - Updated Latest-->
    <xsl:output method="xml" version="1.0" indent="no" />
    <!--  Remove this code starts here  -->
    <xsl:param name="color" />
    <xsl:param name="size">12</xsl:param>
    <xsl:param name="path" />
    <xsl:param name="logopath" />
    <!--  Remove this code ends here  -->
    <xsl:variable name="COLOR">
        <!-- If xsl:param is available assign that value or set default as RED   -->
        <xsl:value-of select="$color" />
        <!--  red -->
    </xsl:variable>
    <xsl:variable name="SIZE">
        <xsl:value-of select="$size" />
        <!-- 12 -->
    </xsl:variable>
    <xsl:variable name="PATH">
        <xsl:value-of select="$path" />
        <!-- 12 -->
    </xsl:variable>
    <xsl:variable name="LOGOPATH">
        <xsl:value-of select="$logopath" />
        <!-- 12 -->
    </xsl:variable>
    <xsl:template match="siteMap">
        <fo:root>
            <xsl:call-template name="siteMap"></xsl:call-template>
        </fo:root>
    </xsl:template>
    <!-- titles -->
    <xsl:attribute-set name="h1">
        <xsl:attribute name="font-size">18pt</xsl:attribute>
        <xsl:attribute name="font-family">"calibri"</xsl:attribute>
        <xsl:attribute name="font-weight">bold</xsl:attribute>
        <xsl:attribute name="space-after">14pt</xsl:attribute>
        <xsl:attribute name="border-after-width">2pt</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="h2">
        <xsl:attribute name="font-size">16pt</xsl:attribute>
        <xsl:attribute name="font-family">"calibri"</xsl:attribute>
        <xsl:attribute name="font-weight">bold</xsl:attribute>
        <xsl:attribute name="space-before">19pt</xsl:attribute>
        <xsl:attribute name="space-after">5pt</xsl:attribute>
        <xsl:attribute name="keep-with-next.within-page">always</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="h3">
        <xsl:attribute name="font-size">
            <xsl:value-of select="$SIZE" />
        </xsl:attribute>
        <xsl:attribute name="color">
            <xsl:value-of select="$COLOR" />
        </xsl:attribute>
        <xsl:attribute name="font-family">"calibri"</xsl:attribute>
        <xsl:attribute name="font-weight">bold</xsl:attribute>
        <xsl:attribute name="space-before">19pt</xsl:attribute>
        <xsl:attribute name="space-after">5pt</xsl:attribute>
        <xsl:attribute name="keep-with-next.within-page">always</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="table.data">
        <xsl:attribute name="table-layout">fixed</xsl:attribute>
        <xsl:attribute name="space-before">10pt</xsl:attribute>
        <xsl:attribute name="space-after">10pt</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="table.data.caption">
        <xsl:attribute name="font-family">calibri</xsl:attribute>
        <xsl:attribute name="text-align">start</xsl:attribute>
        <xsl:attribute name="space-before">3pt</xsl:attribute>
        <xsl:attribute name="space-after">3pt</xsl:attribute>
        <xsl:attribute name="space-after.precedence">2</xsl:attribute>
        <xsl:attribute name="font-weight">bold</xsl:attribute>
        <xsl:attribute name="keep-with-next.within-page">always</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="table.data.th">
        <xsl:attribute name="border-style">solid</xsl:attribute>
        <xsl:attribute name="border-width">1pt</xsl:attribute>
        <xsl:attribute name="padding-start">0.3em</xsl:attribute>
        <xsl:attribute name="padding-end">0.2em</xsl:attribute>
        <xsl:attribute name="padding-before">10pt</xsl:attribute>
        <xsl:attribute name="padding-after">10pt</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="table.data.td">
        <xsl:attribute name="border-style">solid</xsl:attribute>
        <xsl:attribute name="border-width">0pt</xsl:attribute>
        <xsl:attribute name="padding-start">0.3em</xsl:attribute>
        <xsl:attribute name="padding-end">0.2em</xsl:attribute>
        <xsl:attribute name="padding-before">2pt</xsl:attribute>
        <xsl:attribute name="padding-after">2pt</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="toc.line.properties">
        <xsl:attribute name="text-align-last">justify</xsl:attribute>
        <xsl:attribute name="text-align">start</xsl:attribute>
        <xsl:attribute name="end-indent">1.25in</xsl:attribute>
        <xsl:attribute name="last-line-end-indent">-0.25in</xsl:attribute>
    </xsl:attribute-set>
    <xsl:attribute-set name="figtit">
        <xsl:attribute name="keep-together.within-page">always</xsl:attribute>
    </xsl:attribute-set>
    <xsl:variable name="dmcoder">
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@modelIdentCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@systemDiffCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@systemCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@subSystemCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@subSubSystemCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@assyCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@disassyCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@disassyCodeVariant" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@infoCode" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@infoCodeVariant" />
        <xsl:value-of
			select="/dmodule/identAndStatusSection/dmAddress/dmIdent/dmCode/@itemLocationCode" />
    </xsl:variable>
    <xsl:template name="siteMap">
        <!-- <xsl:param name="COLOR" /><xsl:param name="SIZE" /> -->
        <fo:layout-master-set>
            <fo:simple-page-master page-height="297mm"
				page-width="210mm" margin="5mm 5mm 5mm 0mm"
				master-name="PageMaster-Cover">
                <fo:region-body margin="27mm 15mm 23mm 15mm" />
                <fo:region-before region-name="Cover-header"
					extent="21mm" display-align="after" />
                <fo:region-after region-name="Cover-footer"
					extent="20mm" display-align="before" />
                <fo:region-start region-name="Cover-start"
					extent="15mm" />
                <fo:region-end region-name="cover-end" extent="15mm" />
            </fo:simple-page-master>
            <fo:simple-page-master page-height="297mm"
				page-width="210mm" margin="5mm 5mm 5mm 5mm"
				master-name="PageMaster-Left">
                <fo:region-body margin="25mm 12mm 15mm 12mm" />
                <!-- Increased Top and Bottom margin -->
                <fo:region-before region-name="Left-header"
					extent="12mm" display-align="after" />
                <fo:region-after region-name="Left-footer"
					extent="12mm" display-align="before" />
                <fo:region-start region-name="Left-start"
					extent="15mm" />
                <fo:region-end region-name="Left-end" extent="15mm" />
            </fo:simple-page-master>
            <fo:simple-page-master page-height="297mm"
				page-width="210mm" margin="5mm 5mm 5mm 5mm"
				master-name="PageMaster-Right">
                <fo:region-body margin="25mm 12mm 12mm 12mm" />
                <fo:region-before region-name="Right-header"
					extent="12mm" display-align="after" />
                <fo:region-after region-name="Right-footer"
					extent="12mm" display-align="before" />
                <fo:region-start region-name="Right-start"
					extent="15mm" />
                <fo:region-end region-name="Right-end" extent="15mm" />
            </fo:simple-page-master>
            <fo:page-sequence-master master-name="cover">
                <fo:repeatable-page-master-reference
					master-reference="PageMaster-Cover" page-position="first" />
            </fo:page-sequence-master>
            <fo:page-sequence-master
				master-name="PageMaster">
                <fo:repeatable-page-master-alternatives>
                    <fo:conditional-page-master-reference
						master-reference="PageMaster-Left" odd-or-even="even" />
                    <fo:conditional-page-master-reference
						master-reference="PageMaster-Right" odd-or-even="odd" />
                </fo:repeatable-page-master-alternatives>
            </fo:page-sequence-master>
        </fo:layout-master-set>
        <fo:bookmark-tree>
            <xsl:for-each select="siteMapNode/@url">
				<!-- select="document(siteMapNode/@url)/dmodule/identAndStatusSection/dmAddress"> -->
                <xsl:variable name="i" select="position()" />
                <xsl:variable name="current-doc" select="document(.)/dmodule/identAndStatusSection/dmAddress" />
                <xsl:for-each select="$current-doc">
                    <xsl:for-each select="//dmTitle">
                        <xsl:if test="name(parent::node())='dmAddressItems'">
                            <fo:bookmark internal-destination="{generate-id(.)}"
                                starting-state="show">
                                <fo:bookmark-title>
                                    <xsl:value-of select="$i" />
                                    <xsl:text>. </xsl:text>
                                    <xsl:value-of select="infoName" />
                                </fo:bookmark-title>
                            </fo:bookmark>
                        </xsl:if>
                    </xsl:for-each>
                </xsl:for-each>
            </xsl:for-each>
        </fo:bookmark-tree>
        <fo:page-sequence master-reference="cover">
            <!--	<fo:static-content flow-name="Cover-header"><fo:block text-align="center" color="black" font-size="10pt" font-weight="bold"><fo:inline text-decoration="underline"><xsl:text>CONFIDENTIAL</xsl:text></fo:inline></fo:block></fo:static-content><fo:static-content flow-name="Cover-footer"><fo:block text-align="center" color="black"  font-size="10pt" font-weight="bold" text-decoration="underline" ><fo:inline text-decoration="underline"><xsl:text>CONFIDENTIAL</xsl:text></fo:inline></fo:block></fo:static-content>    -->
            <!-- Main pages start here  (Newly Added)   -->
            <fo:flow flow-name="xsl-region-body" >
                <fo:block page-break-inside="avoid">
                    <fo:block padding-top="10pt" padding-bottom="3pt">
                        <fo:table padding-bottom="10pt" padding-top="10pt" margin-left="17pt" margin-right="8pt">
                            <fo:table-column column-width="100%" />
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell  background-color="#009900">
                                        <fo:block text-align="center">
                                            <fo:block  padding-top="20pt"  padding-bottom="20pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
				CONFIDENTIAL
			</fo:block>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                       </fo:table>
                    </fo:block>
                    <fo:block padding-top="1pt">
                        <fo:table padding-bottom="1pt" padding-top="1pt" margin-left="17pt" margin-right="8pt">
                            <fo:table-column column-width="60%"/>
                            <fo:table-column column-width="40%"/>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell  background-color="#598BAF">
                                        <fo:block text-align="center">
                                            <fo:block  padding-top="15pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
                                                <xsl:for-each
						         select="siteMapNode/@manual">
                                                    <fo:inline>
                                                        <xsl:value-of select="." />
                                                    </fo:inline>
                                                </xsl:for-each>
                                            </fo:block>
                                            <fo:block  padding-top="5pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
				For
			</fo:block>
                                            <fo:block  padding-top="5pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
                                                <xsl:for-each
						select="siteMapNode/@module">
                                                    <fo:inline>
                                                        <xsl:value-of select="." />
                                                    </fo:inline>
                                                </xsl:for-each>
                                            </fo:block>
                                            <fo:block  padding-top="5pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center"><xsl:for-each
						select="document(siteMapNode[1]/@url)/dmodule/identAndStatusSection/dmStatus/dataRestrictions/restrictionInfo/copyright/copyrightPara/randomList/listItem/para[@commercialClassification='cc02']"><fo:inline><xsl:value-of select="." /></fo:inline></xsl:for-each></fo:block>
                                            <fo:block  padding-top="15pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
                                                <xsl:for-each
						         select="siteMapNode/@eqpt">
                                                    <fo:inline>
                                                        <xsl:value-of select="." />
                                                    </fo:inline>
                                                </xsl:for-each>
                                            </fo:block>
                                        </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  background-color="#be514e">
                                        <fo:block text-align="center">
                                            <fo:external-graphic content-width="40%" content-height="100%" scaling="uniform">
                                                <xsl:attribute name="src">
                                                    <xsl:value-of select="concat($LOGOPATH, 'mainlogo.png')" />
                                                </xsl:attribute>
                                            </fo:external-graphic>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                    <fo:block  page-break-inside="avoid" start-indent="17pt"  end-indent="8pt" text-align="center">
                        <fo:external-graphic   content-width="80%" content-height="100%" src="ControlRoom.png"/>
                    </fo:block>
                    <fo:block>
                        <fo:table padding-bottom="10pt" padding-top="10pt" margin-left="17pt" margin-right="8pt">
                            <fo:table-column column-width="100%" />
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell  background-color="#009900">
                                        <fo:block text-align="center">
                                            <fo:block  padding-top="20pt"  padding-bottom="20pt" font-weight="bold" font-family="Arial" font-size="16pt" color="black" text-align="center">
				CONFIDENTIAL
			</fo:block>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                </fo:block>
                <fo:block page-break-before="always">
                    <fo:inline-container>
                        <fo:block text-align="center"  padding-bottom="3pt" padding-top="250pt" start-indent="150pt" end-indent="150pt" >
                            <fo:block border="1pt" border-style="solid" border-color="black"  padding="5pt" font-family="Arial" font-size="12pt" color="black" text-align="center">
                          This page is intentionally left blank
                  </fo:block>
                        </fo:block>
                    </fo:inline-container>
                </fo:block>
                <fo:block page-break-before="always">
                    <fo:block >
                        <fo:table >
                            <fo:table-column column-width="68%" />
                            <fo:table-column column-width="15%" />
                            <fo:table-column column-width="1%" />
                            <fo:table-column column-width="16%" />
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="left"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="right">Document No.</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="left">&#160;:&#160;</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell    border="0pt solid black" font-family="Tahoma"   >
                                        <fo:block  font-size="12pt" text-align="left">&#160;
                                            <xsl:for-each
						select="siteMapNode/@document">
                                                <fo:inline>
                                                    <xsl:value-of select="." />
                                                </fo:inline>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                                <fo:table-row>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="left"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="right">Version No.</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block  font-size="12pt" text-align="left">&#160;:&#160;</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell    border="0pt solid black" font-family="Tahoma" >
                                        <fo:block  font-size="12pt" text-align="left">&#160;
                                            <xsl:for-each
						select="siteMapNode/@version">
                                                <fo:inline>
                                                    <xsl:value-of select="." />
                                                </fo:inline>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                                <fo:table-row>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="left"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block font-size="12pt" text-align="right">Date</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block  font-size="12pt" text-align="left">&#160;:&#160;</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell   border="0pt solid black" font-family="Tahoma" >
                                        <fo:block padding-bottom="5px" font-size="12pt" text-align="left">&#160;
                                            <xsl:for-each
						select="siteMapNode/@date">
                                                <fo:inline>
                                                    <xsl:value-of select="." />
                                                </fo:inline>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                    <fo:block text-align="center" start-indent="5pt" end-indent="5pt">
                        <fo:block border-top="3px solid black" padding-top="5pt" font-weight="bold" font-family="Arial" font-size="19pt" color="#1a4f91" text-align="center">
                            <xsl:for-each
						         select="siteMapNode/@manual">
                                <fo:inline>
                                    <xsl:value-of select="." />
                                </fo:inline>
                            </xsl:for-each>
                        </fo:block>
                        <fo:block  padding-top="5pt" font-weight="bold" font-family="Arial" font-size="19pt" color="#1a4f91" text-align="center">
                            <xsl:for-each
						select="siteMapNode/@module">
                                <fo:inline>
                                    <xsl:value-of select="." />
                                </fo:inline>
                            </xsl:for-each>
                        </fo:block>
                        <fo:block border-bottom="3px solid black" padding-bottom="5pt"></fo:block>
                    </fo:block>
                    <fo:block padding-bottom="5pt" padding-top="10pt" color="#1a4f91" font-size="16pt" text-align="center">
    For
</fo:block>
                    <fo:block padding-bottom="10pt" padding-top="2pt"  font-weight="bold" color="#1a4f91" font-size="16pt" text-align="center">
                        <xsl:for-each
						         select="siteMapNode/@eqpt">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
                    </fo:block>
                    <fo:block text-align="center" page-break-inside="avoid" padding-bottom="20pt" padding-top="20pt" >
                        <fo:external-graphic  content-width="40%" content-height="20%" src="OEMLogo.png"/>
                    </fo:block>
                    <fo:block padding-bottom="0pt" padding-top="10pt" color="black" font-size="12pt" text-align="center">
    Prepared By
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="10pt" color="black" font-size="12pt" text-align="center">
    <xsl:for-each
						         select="siteMapNode/@prepared_by">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="10pt" color="black" font-size="12pt" text-align="center">
    Prepared For
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="10pt" color="black" font-size="12pt" text-align="center">
    <xsl:for-each
						         select="siteMapNode/@prepared_for">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                    <fo:block padding-bottom="25pt" padding-top="10pt" color="black" font-size="12pt" text-align="center">
    <xsl:for-each
						         select="siteMapNode/@prepared_by">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each> owns the copyright of this document that is supplied in confidence and which must not be used for any other purpose other than that for which it is supplied and must not be reproduced without the written permission from the copyright holders. 
</fo:block>
                    <fo:block text-align="center" start-indent="75pt" end-indent="75pt">
                        <fo:block border="0.2pt" border-style="solid" border-color="black" font-weight="bold" padding="5pt" font-family="Arial" font-size="13pt" color="red" text-align="justify">
        THIS DOCUMENT VERSION 
		
                            
                            
                            
                            <xsl:for-each select="siteMapNode/@version">
                                <fo:inline>
                                    <xsl:value-of select="." />
                                </fo:inline>
                            </xsl:for-each> SUPERSEDES ALL THE PREVIOUS VERSIONS OF THIS DOCUMENT.
    
                        
                        
                        
                        </fo:block>
                    </fo:block>
                    <fo:block padding-bottom="0pt" padding-top="25pt" color="black" font-size="10pt" text-align="center">
    <xsl:for-each
						         select="siteMapNode/@address1">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="0pt" color="black" font-size="10pt" text-align="center">
   <xsl:for-each
						         select="siteMapNode/@address2">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="0pt" color="black" font-size="10pt" text-align="center">
    <xsl:for-each
						         select="siteMapNode/@address3">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                    <fo:block padding-bottom="0pt" padding-top="0pt" color="black" font-size="10pt" text-align="center">
   <xsl:for-each
						         select="siteMapNode/@address4">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
<fo:block padding-bottom="0pt" padding-top="0pt" color="black" font-size="10pt" text-align="center">
   <xsl:for-each
						         select="siteMapNode/@address5">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
<fo:block padding-bottom="0pt" padding-top="0pt" color="black" font-size="10pt" text-align="center">
   <xsl:for-each
						         select="siteMapNode/@address6">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each>
</fo:block>
                </fo:block>
                <fo:block page-break-before="always"  >
                    <fo:inline-container>
                        <fo:block text-align="center"  padding-bottom="3pt" padding-top="250pt" start-indent="150pt" end-indent="150pt" >
                            <fo:block border="1pt" border-style="solid" border-color="black"  padding="5pt" font-family="Arial" font-size="12pt"
        color="black" text-align="center">
        This page is intentionally left blank
    </fo:block>
                        </fo:block>
                    </fo:inline-container>
                </fo:block>
            </fo:flow>
        </fo:page-sequence>
        <fo:page-sequence master-reference="PageMaster">
            <!-- even page -->
            <fo:static-content flow-name="Left-header">
                <fo:block  padding-bottom="3pt" padding-top="3pt" text-align="center" font-family="calibri" font-size="9pt">
                    <fo:inline text-decoration="underline">
                        <xsl:text>CONFIDENTIAL</xsl:text>
                    </fo:inline>
                </fo:block>
                <fo:block padding-bottom="10pt" >
                    <fo:table border="2pt" border-style="solid" padding-bottom="10pt" padding-top="10pt">
                        <fo:table-column column-width="30%"/>
                        <fo:table-column column-width="70%"/>
                        <fo:table-body>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block text-align="left">
                                        <fo:external-graphic src="OEMLogo.png" content-width="20%" content-height="10%" />
                                    </fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold" font-size="12pt">
                                    <fo:block  padding-top="9pt"   text-align="center">
                                        <xsl:for-each
						         select="siteMapNode/@manual">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                        </fo:table-body>
                    </fo:table>
                </fo:block>
            </fo:static-content>
            <!-- Right page header -->
            <!-- odd page -->
            <fo:static-content flow-name="Right-header">
                <fo:block  padding-bottom="3pt" padding-top="3pt" text-align="center" font-family="calibri" font-size="9pt">
                    <fo:inline text-decoration="underline">
                        <xsl:text>CONFIDENTIAL</xsl:text>
                    </fo:inline>
                </fo:block>
                <fo:block padding-bottom="10pt" >
                    <fo:table border="2pt" border-style="solid" padding-bottom="10pt" padding-top="10pt">
                        <fo:table-column column-width="30%"/>
                        <fo:table-column column-width="70%"/>
                        <fo:table-body>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block text-align="left">
                                        <fo:external-graphic src="OEMLogo.png" content-width="20%" content-height="10%" text-align="left"/>
                                    </fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold" font-size="12pt">
                                    <fo:block  padding-top="9pt" text-align="center">
                                        <xsl:for-each
						         select="siteMapNode/@manual">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                        </fo:table-body>
                    </fo:table>
                </fo:block>
            </fo:static-content>
            <!--     Side wise name alignment   -->
            <!--	<fo:static-content flow-name="Right-end"><fo:block-container reference-orientation="90"><fo:block text-align="left" font-size="9pt"
						padding-top="30pt"  margin-left="100pt"> Printed
						using NavIETM </fo:block></fo:block-container></fo:static-content><fo:static-content flow-name="Left-start"><fo:block-container reference-orientation="90"><fo:block text-align="left" font-size="9pt"
						 padding-bottom="20pt" margin-left="100pt"> Printed
						using NavIETM </fo:block></fo:block-container></fo:static-content>  -->
            <!--    Odd page footer  (Newly added -->
            <!-- odd page -->
            <fo:static-content flow-name="Right-footer">
                <fo:block>
                    <fo:table border="2pt" border-style="solid" padding-bottom="10pt" padding-top="10pt">
                        <fo:table-column column-width="20%"/>
                        <fo:table-column column-width="71%"/>
                        <fo:table-column column-width="10%"/>
                        <fo:table-body>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block>Project Name</fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block   color="black" font-size="10pt" text-align="center">
                                        <xsl:for-each select="siteMapNode/@eqpt">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>                               
                            </fo:table-row>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block>Document Name</fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block  color="black" font-size="10pt" text-align="center">
                                        <xsl:for-each select="siteMapNode/@module">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                    <fo:block  color="black" font-size="3pt" text-align="center" >
                                        <fo:retrieve-marker retrieve-class-name="section.footer.marker.right1" retrieve-position="first-including-carryover" retrieve-boundary="page-sequence" />
                                    </fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                        </fo:table-body>
                    </fo:table>
                </fo:block>
                <fo:block  padding-bottom="3pt" padding-top="3pt" text-align="center" font-family="calibri" font-size="9pt">
                    <fo:inline text-decoration="underline">
                        <xsl:text>CONFIDENTIAL</xsl:text>
                    </fo:inline>
                </fo:block>
            </fo:static-content>
            <!-- even page  footer     (Newly Added) -->
			
            <!-- even page -->
            <fo:static-content flow-name="Left-footer" >
                <fo:block  >
                    <fo:table border="2pt" border-style="solid" padding-bottom="10pt" padding-top="10pt">
                        <fo:table-column column-width="20%"/>
                        <fo:table-column column-width="71%"/>
                        <fo:table-column column-width="10%"/>
                        <fo:table-body>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block>Project Name</fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block   color="black" font-size="10pt" text-align="center">
                                        <xsl:for-each select="siteMapNode/@eqpt">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block>Page No.</fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                            <fo:table-row>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block>Document Name</fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                    <fo:block   color="black" font-size="10pt" text-align="center">
                                        <xsl:for-each select="siteMapNode/@module">
                                            <fo:inline>
                                                <xsl:value-of select="." />
                                            </fo:inline>
                                        </xsl:for-each>
                                    </fo:block>
                                </fo:table-cell>
                                <fo:table-cell border="1pt solid black" font-family="Arial"  >
                                    <fo:block  color="black"  font-size="1pt" text-align="center" >
                                        <fo:retrieve-marker	retrieve-class-name="section.footer.marker.right1" retrieve-position="first-including-carryover" retrieve-boundary="page-sequence" />
                                    </fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                        </fo:table-body>
                    </fo:table>
                </fo:block>
                <fo:block  padding-bottom="3pt" padding-top="3pt" text-align="center" font-family="calibri" font-size="9pt">
                    <fo:inline text-decoration="underline">
                        <xsl:text>CONFIDENTIAL</xsl:text>
                    </fo:inline>
                </fo:block>
            </fo:static-content>
            <fo:flow flow-name="xsl-region-body">
                <fo:block page-break-inside="avoid" >
                    <fo:block padding-bottom="30pt" padding-top="40pt"
					font-size="14pt" font-weight="bold" text-align="center" text-decoration="underline" font-family="Tahoma">
					Symbols and Text Format Used in This Document
				</fo:block>
                    <fo:block >
                        <fo:table border="1pt solid black" padding-bottom="10pt" padding-top="20pt" start-indent="10pt" end-indent="10pt">
                            <fo:table-column column-width="25%" />
                            <fo:table-column column-width="75%" />
                            <fo:table-header>
                                <fo:table-row background-color="#D8D8D8">
                                    <fo:table-cell display-align="after" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Symbol/Text Format </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  padding-bottom="10pt" font-size="12pt" text-align="center">Denotes </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-header>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma">
                                        <fo:block text-align="center">
                                            <fo:external-graphic src="Note.png" width="50%" content-width="scale-to-fit"/>
                                        </fo:block>
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">NOTE</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Information that needs extra attention from the user/operator.</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block text-align="center">
                                            <fo:external-graphic src="Warning.png" width="50%" content-width="scale-to-fit"/>
                                        </fo:block>
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">WARNING </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Information that needs cautions user against potential fallout.</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Courier New">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Courier New font</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Text for code, command, or output.</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center"> Bold text in procedures</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Text, which user has to click on the screen to execute the procedure.</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Click</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Unless specified click/clicking means a single, left-click. Wherever applicable, a double-click (for example, to start installation using an exe) or a right-click (for example, to display the context menu), is explicitly mentioned.</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                </fo:block>
                <fo:block page-break-before="always"  >
                    <fo:inline-container>
                        <fo:block text-align="center"  padding-bottom="3pt" padding-top="250pt" start-indent="150pt" end-indent="150pt" >
                            <fo:block border="1pt" border-style="solid" border-color="black"  padding="5pt" font-family="Arial" font-size="12pt"
        color="black" text-align="center">
        This page is intentionally left blank
    </fo:block>
                        </fo:block>
                    </fo:inline-container>
                </fo:block>
                <fo:block page-break-before="always"  >
                    <fo:block padding-bottom="30pt" padding-top="50pt"
					font-size="14pt" font-weight="bold" text-align="center" text-decoration="underline" font-family="Tahoma">
					CONTRIBUTORS AND APPROVER
				</fo:block>
                    <fo:block >
                        <fo:table border="1pt solid black" padding-bottom="10pt" padding-top="20pt" start-indent="10pt" end-indent="10pt">
                            <fo:table-column column-width="30%" />
                            <fo:table-column column-width="70%" />
                            <fo:table-header>
                                <fo:table-row background-color="#D8D8D8">
                                    <fo:table-cell display-align="after" border="1pt solid black" font-weight="bold" text-align="center" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Contribution</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" border="1pt solid black" font-weight="bold" text-align="center" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center">Contributor/Approver</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-header>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="10pt" text-align="center">Prepared by</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="10pt" text-align="center">Reviewed by <xsl:for-each
						         select="siteMapNode/@reviewed_by1">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" font-weight="bold">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="10pt" text-align="center">Reviewed by <xsl:for-each
						         select="siteMapNode/@reviewed_by2">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma">
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="10pt" text-align="center">Approved by <xsl:for-each
						         select="siteMapNode/@approved_by">
                            <fo:inline>
                                <xsl:value-of select="." />
                            </fo:inline>
                        </xsl:for-each></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                </fo:block>
                <fo:block page-break-before="always"  >
                    <fo:inline-container>
                        <fo:block text-align="center"  padding-bottom="3pt" padding-top="250pt" start-indent="150pt" end-indent="150pt" >
                            <fo:block border="1pt" border-style="solid" border-color="black"  padding="5pt" font-family="Arial" font-size="12pt"
        color="black" text-align="center">
        This page is intentionally left blank
    </fo:block>
                        </fo:block>
                    </fo:inline-container>
                </fo:block>
                <fo:block page-break-before="always">
                    <fo:block padding-bottom="30pt" padding-top="50pt"
					font-size="14pt" font-weight="bold" text-align="center" text-decoration="underline" font-family="Tahoma">
					CONTROL SHEET
				</fo:block>
                    <fo:block >
                        <fo:table border="1pt solid black" padding-bottom="10pt" padding-top="20pt" start-indent="10pt" end-indent="10pt">
                            <fo:table-column column-width="10%" />
                            <fo:table-column column-width="10%" />
                            <fo:table-column column-width="15%" />
                            <fo:table-column column-width="18%" />
                            <fo:table-column column-width="20%" />
                            <fo:table-column column-width="27%" />
                            <fo:table-header>
                                <fo:table-row background-color="#D8D8D8">
                                    <fo:table-cell display-align="after" padding-bottom="10pt" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">Sr. No.</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" padding-bottom="17pt"  border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">Version</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" padding-bottom="17pt" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">Date</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" padding-bottom="17pt" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">Milestone</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" padding-bottom="17pt" border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">No. of Pages</fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after"  border="1pt solid black" font-weight="bold" text-align="centre" font-family="Tahoma">
                                        <fo:block  font-size="12pt" text-align="center">Change Description and
 Reason for Change</fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-header>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                            <fo:table-body>
                                <fo:table-row>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="10pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell  border="1pt solid black" font-family="Tahoma" >
                                        <fo:block padding-top="15pt" padding-bottom="15pt" font-size="12pt" text-align="center"></fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                </fo:block>
                <fo:block page-break-before="always"  >
                    <fo:inline-container>
                        <fo:block text-align="center"  padding-bottom="3pt" padding-top="250pt" start-indent="150pt" end-indent="150pt" >
                            <fo:block border="1pt" border-style="solid" border-color="black"  padding="5pt" font-family="Arial" font-size="12pt"
        color="black" text-align="center">
        This page is intentionally left blank
    </fo:block>
                        </fo:block>
                    </fo:inline-container>
                </fo:block>
                <!-- Main Pages end here -->
                <!--CONTENTS SECTION (TOC,LOT,LOF)-->
                <fo:block page-break-before="always" text-align="center">
                    <fo:block padding-bottom="20pt" padding-top="30pt" text-decoration="underline"
						font-size="20pt" font-weight="bold" text-align="center"> Contents
					</fo:block>
                    <xsl:value-of
						select="mycode:setFileNum(count(siteMapNode))" />

                        
                    <xsl:for-each select="siteMapNode/@url">
                        <xsl:variable name="i" select="position()" />
                        <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                        <xsl:for-each select="$current-doc">
                            <fo:block padding-left="30pt" color="blue"
							    padding-bottom="10pt" font-size="12pt">
                                <xsl:for-each select="//dmTitle">
                                    <xsl:if test="name(parent::node())='dmAddressItems'">
                                        <fo:block text-align-last="justify" font-weight="bold">
                                            <fo:basic-link
											    internal-destination="{generate-id(.)}">
											    Chapter 
                                            
                                            
                                            
                                                <xsl:value-of select="$i" />: 
											
                                            
                                            
                                            
                                                <xsl:value-of select="infoName" />
                                                <fo:leader leader-pattern="dots" />
                                                <xsl:value-of select="$i" />.
                                            
                                            
                                            
                                                <fo:page-number-citation ref-id="{generate-id(.)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </xsl:if>
                                </xsl:for-each>
                                <xsl:for-each select="//levelledPara/title">
                                    <xsl:if test="//levelledPara/title">
                                        <fo:block text-align-last="justify" start-indent="15pt" color="#000000">
                                            <fo:basic-link
											    internal-destination="{generate-id(..)}">
                                                <xsl:value-of select="$i" />-
                                            
                                            
                                            
                                                <xsl:value-of select="count(preceding::title) + 1" />:
											 
                                            
                                            
                                            
                                                <xsl:value-of select="." />
                                                <xsl:if test="graphic">
                                                    <xsl:value-of
														    select="mycode:setFigs(number(substring(@id,5)), count(preceding::levelledPara) + 1) " />
                                                </xsl:if>
                                                <fo:leader leader-pattern="dots" />
                                                <xsl:value-of select="$i" />-
                                            
                                            
                                            
                                                <fo:page-number-citation ref-id="{generate-id(..)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </xsl:if>
                                </xsl:for-each>
                                <xsl:for-each select="//proceduralStep/title">
                                    <xsl:if test="//proceduralStep/title">
                                        <fo:block text-align-last="justify" color="#000000" start-indent="15pt">
                                            <fo:basic-link
											    internal-destination="{generate-id(..)}">
                                                <xsl:value-of select="$i" />-
                                            
                                            
                                            
                                                <xsl:value-of select="count(preceding::title) + 1" />:
											 
                                            
                                            
                                            
                                                <xsl:value-of select="." />
                                                <xsl:if test="graphic">
                                                    <xsl:value-of
														    select="mycode:setFigs(number(substring(@id,5)), count(preceding::proceduralStep) + 1) " />
                                                </xsl:if>
                                                <fo:leader leader-pattern="dots" />
                                                <xsl:value-of select="$i" />-
                                            
                                                <fo:page-number-citation ref-id="{generate-id(..)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </xsl:if>
                                </xsl:for-each>
                            </fo:block>
						</xsl:for-each>
                    </xsl:for-each>
                </fo:block>
                <!-- LOF WORKING-->
                <fo:block  padding-top="20pt" text-align="center" page-break-before="always" >
                    <fo:block padding-bottom="20pt" padding-top="20pt" text-decoration="underline"
						font-size="20pt" font-weight="bold" text-align="center"> List Of Figures
					</fo:block>
                    <xsl:value-of
						select="mycode:setFileNum(count(siteMapNode))" />
                    <xsl:for-each select="siteMapNode/@url">
						<!-- select="document(siteMapNode/@url)/dmodule/content"> -->
                        <xsl:variable name="i" select="position()" />
                        <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                        <xsl:for-each select="$current-doc">
                            <fo:block padding-left="20pt" color="black"
                                font-size="12pt">
                                <xsl:for-each select="//figure">
                                    <xsl:if test="//levelledPara|//proceduralStep|//illustratedPartsCatalog">
                                        <fo:block text-align-last="justify">
                                            <fo:basic-link
                                                internal-destination="{generate-id(.)}">
                                                Figure 
                                                
                                                
                                                
                                                <xsl:value-of select="$i" />-
                                                
                                                
                                                
                                                <xsl:value-of select="count(preceding::figure) + 1" />: 
                                                
                                                
                                                
                                                
                                                <xsl:value-of select="title" />
                                                <fo:leader leader-pattern="dots" />
                                                <xsl:value-of select="$i" />-
                                                
                                                
                                                
                                                <fo:page-number-citation ref-id="{generate-id(.)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </xsl:if>
                                </xsl:for-each>
                            </fo:block>
                        </xsl:for-each>
                    </xsl:for-each>
                </fo:block>
                <!-- LOT WORKING-->
                <fo:block text-align="center" page-break-before="always" >
                    <fo:block padding-bottom="20pt" padding-top="20pt" text-decoration="underline"
						font-size="20pt" font-weight="bold" text-align="center"> List Of Tables
					</fo:block>
                    <xsl:for-each select="siteMapNode/@url">
						<!-- select="document(siteMapNode/@url)/dmodule/content"> -->
                        <xsl:variable name="i" select="position()" />
                        <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                        <xsl:for-each select="$current-doc">
                            <fo:block padding-left="20pt" color="black"
                                font-size="12pt">
                                <xsl:for-each select="//table">
                                    <xsl:if test="//levelledPara|//proceduralStep">
                                        <fo:block text-align-last="justify">
                                            <fo:basic-link
                                                internal-destination="{generate-id(.)}">
                                                
                                                Table 
                                                
                                                
                                                
                                                <xsl:value-of select="$i" />-
                                                
                                                
                                                
                                                <xsl:value-of select="count(preceding::table) + 1" />: 
                                                
                                                
                                                
                                                
                                                <xsl:value-of select="title" />
                                                <fo:leader leader-pattern="dots" />
                                                <xsl:value-of select="$i" />-
                                                
                                                
                                                
                                                <fo:page-number-citation ref-id="{generate-id(.)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </xsl:if>
                                </xsl:for-each>
                            </fo:block>
                        </xsl:for-each>
                    </xsl:for-each>
                </fo:block>
                <fo:block page-break-before="always" >
                    <fo:block padding-bottom="10pt" padding-top="20pt"
					font-size="11.5pt" font-weight="bold" text-align="center"  font-family="Times New Roman">
					LIST OF ABBREVIATIONS
				</fo:block>
                    <fo:block >
                        <fo:table  padding-bottom="10pt" padding-top="20pt" display-align="before" 
			text-align="start" inline-progression-dimension="auto">
                            <fo:table-column column-width="5%" />
                            <fo:table-column column-width="25%" />
                            <fo:table-column column-width="70%" />
                            <fo:table-body>
                                <fo:table-row >
                                    <fo:table-cell  >
                                        <fo:block padding-top="5pt" padding-bottom="5pt" wrap-option="wrap">
                                            <xsl:value-of
						select="mycode:setFileNum(count(siteMapNode))" />
                                            <xsl:for-each select="siteMapNode/@url">
                                                <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                                                <xsl:for-each select="$current-doc">
                                                <!-- select="document(siteMapNode/@url)/dmodule/content"> -->
                                                    <fo:block padding-left="20pt" color="black" font-size="12pt">
                                                        <xsl:for-each select="//acronymTerm">
                                                            <xsl:if test="//levelledPara|//proceduralStep">
                                                                <fo:block text-align-last="left">
                                                                    <fo:basic-link internal-destination="{generate-id(.)}">
                                                                        <xsl:value-of select="count(preceding::acronymTerm) + 1" />
                                                                    </fo:basic-link>
                                                                </fo:block>
                                                            </xsl:if>
                                                        </xsl:for-each>
                                                    </fo:block>
                                                </xsl:for-each>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" >
                                        <fo:block padding-top="5pt" padding-bottom="10pt" wrap-option="wrap" >
                                            <xsl:for-each select="siteMapNode/@url">
                                                <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                                                <xsl:for-each select="$current-doc">
                                                <!-- select="document(siteMapNode/@url)/dmodule/content"> -->
                                                    <fo:block padding-left="20pt" color="black" font-size="12pt">
                                                        <xsl:for-each select="//acronym">
                                                            <xsl:if test="//levelledPara|//proceduralStep">
                                                                <fo:block text-align-last="left">
                                                                    <fo:basic-link internal-destination="{generate-id(.)}">
                                                                        <xsl:value-of select="acronymTerm" />
                                                                    </fo:basic-link>
                                                                </fo:block>
                                                            </xsl:if>
                                                        </xsl:for-each>
                                                    </fo:block>
                                                </xsl:for-each>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell display-align="after" >
                                        <fo:block padding-top="5pt" padding-bottom="10pt" wrap-option="wrap" >
                                            <xsl:for-each select="siteMapNode/@url">
                                                <xsl:variable name="current-doc" select="document(.)/dmodule/content" />
                                                <xsl:for-each select="$current-doc">
                                                <!-- select="document(siteMapNode/@url)/dmodule/content"> -->
                                                    <fo:block padding-left="20pt" color="black" font-size="12pt">
                                                        <xsl:for-each select="//acronym">
                                                            <xsl:if test="//levelledPara|//proceduralStep">
                                                                <fo:block text-align-last="left">
                                                                    <fo:basic-link internal-destination="{generate-id(.)}">
                                                                        <xsl:text> :</xsl:text>
                                                                        <xsl:value-of select="acronymDefinition" />
                                                                    </fo:basic-link>
                                                                </fo:block>
                                                            </xsl:if>
                                                        </xsl:for-each>
                                                    </fo:block>
                                                </xsl:for-each>
                                            </xsl:for-each>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </fo:table-body>
                        </fo:table>
                    </fo:block>
                </fo:block>
                <xsl:for-each select="siteMapNode/@url">
                    <xsl:variable name="i" select="position()" />
                    <xsl:variable name="current-doc" select="document(.)" />
                    <xsl:for-each select="$current-doc">
                    <!-- select="document(siteMapNode/@url)"> -->
                        <xsl:value-of select="mycode:setFileNum($i)" />
                        <xsl:apply-templates select="dmodule" />
                    </xsl:for-each>
                </xsl:for-each>
            </fo:flow>
        </fo:page-sequence>
    </xsl:template>
    <xsl:template name="mapMonth">
        <xsl:param name="monthNum"/>
        <xsl:choose>
            <xsl:when test="$monthNum = '01'">January</xsl:when>
            <xsl:when test="$monthNum = '02'">February</xsl:when>
            <xsl:when test="$monthNum = '03'">March</xsl:when>
            <xsl:when test="$monthNum = '04'">April</xsl:when>
            <xsl:when test="$monthNum = '05'">May</xsl:when>
            <xsl:when test="$monthNum = '06'">June</xsl:when>
            <xsl:when test="$monthNum = '07'">July</xsl:when>
            <xsl:when test="$monthNum = '08'">August</xsl:when>
            <xsl:when test="$monthNum = '09'">September</xsl:when>
            <xsl:when test="$monthNum = '10'">October</xsl:when>
            <xsl:when test="$monthNum = '11'">November</xsl:when>
            <xsl:when test="$monthNum = '12'">December</xsl:when>
            <!-- Add more cases for other months as needed -->
            <xsl:otherwise>Unknown</xsl:otherwise>
            <!-- Default case if not recognized -->
        </xsl:choose>
    </xsl:template>
    <xsl:template match="dmAddress">
        <fo:block padding-left="40pt" color="blue"
			padding-bottom="15pt" font-size="12pt">
            <xsl:for-each select="//dmTitle">
                <xsl:if test="name(parent::node())='dmAddressItems'">
                    <fo:block text-align-last="justify">
                        <fo:basic-link
							internal-destination="{generate-id(.)}">
                            <xsl:variable name="i" select="position()" />
                            <xsl:value-of select="$i" />
                            <fo:inline space-end="30pt" />
                            <xsl:value-of select="infoName" />
                            <fo:leader leader-pattern="dots" />
                            <fo:page-number-citation
								ref-id="{generate-id(.)}" />
                        </fo:basic-link>
                    </fo:block>
                </xsl:if>
            </xsl:for-each>
        </fo:block>
    </xsl:template>
    <xsl:template match="dmodule">
        <xsl:call-template name="genTOC" />
        <fo:block break-before="always">
            <fo:marker marker-class-name="section.footer.marker.left">
                <xsl:for-each
					select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle">
                    <fo:block text-align="left" font-size="8pt" font-family="calibri" font-style="italic" >
                        <xsl:value-of select="techName" />
                    </fo:block>
                </xsl:for-each>
            </fo:marker>
            <fo:marker marker-class-name="section.footer.marker.left1">
                <xsl:for-each
					select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle">
                    <fo:block text-align="center" font-size="12pt" font-family="calibri">
                        <xsl:value-of select="mycode:getFileNum()" />-
                        
                        
                        
                        <fo:page-number/>
                    </fo:block>
                </xsl:for-each>
            </fo:marker>
        </fo:block>
		<xsl:apply-templates select="//dmTitle"/>
        <xsl:apply-templates select="content" />
    </xsl:template>
    <!-- to link content -->
    <xsl:template match="dmTitle">
        <xsl:if test="name(parent::node())='dmAddressItems'">
            <fo:block id="{generate-id(.)}" color="blue"></fo:block>
        </xsl:if>
    </xsl:template>
    <xsl:template name="genTOC">
        <fo:block  break-before="odd-page">
            <fo:marker marker-class-name="section.footer.marker.right">
                <xsl:for-each
					select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle">
                    <fo:block text-align="left" font-size="8pt" font-family="calibri" font-style="italic" >
                        <xsl:value-of select="techName" />
                    </fo:block>
                </xsl:for-each>
            </fo:marker>
            <fo:marker marker-class-name="section.footer.marker.right1">
                <xsl:for-each
					select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle">
                    <fo:block text-align="center" font-size="12pt" font-family="calibri">
                        <xsl:value-of select="mycode:getFileNum()" />-
                        
                        
                        
                        <fo:page-number/>
                    </fo:block>
                </xsl:for-each>
            </fo:marker>
            <xsl:variable name="dmref">
                <xsl:for-each
					select="identAndStatusSection/dmAddress/dmIdent/dmCode">
                    <xsl:value-of select="@modelIdentCode" />-
                    
                    
                    
                    <xsl:value-of select="@systemDiffCode" />-
                    
                    
                    
                    <xsl:value-of select="@systemCode" />-
                    
                    
                    
                    <xsl:value-of select="@subSystemCode" />
                    <xsl:value-of select="@subSubSystemCode" />-
                    
                    
                    
                    <xsl:value-of select="@assyCode" />-
                    
                    
                    
                    <xsl:value-of select="@disassyCode" />
                    <xsl:value-of select="@disassyCodeVariant" />-
                    
                    
                    
                    <xsl:value-of select="@infoCode" />
                    <xsl:value-of select="@infoCodeVariant" />-
                    
                    
                    
                    <xsl:value-of select="@itemLocationCode" />
                </xsl:for-each>
            </xsl:variable>
             <fo:block  xsl:use-attribute-sets="h1" text-align="center" padding-top="150"
				display-align="center" font-weight="bold" font-size="60" >
				CHAPTER 
				
				
               
                
                
                <xsl:value-of select="mycode:getFileNum()" />
				 <fo:block  xsl:use-attribute-sets="h1" text-align="center" padding-top="10"
				display-align="center" font-weight="bold" font-size="50" >
               
                <xsl:value-of
					select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle/infoName" />
            </fo:block>
    </fo:block>
            <xsl:if test="content/isolationProcedure">
                <fo:table >
                    <fo:table-column column-width="85%" />
                    <fo:table-column column-width="15%" />
                    <fo:table-body>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-bottom="5pt" text-align="left" font-size="14pt" font-weight="bold">
										Table of Contents
									</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block font-size="10pt" text-align="right" font-weight="bold">
										Page
									</fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                    </fo:table-body>
                </fo:table>
                <fo:block margin-left="20pt" color="blue"
					padding-bottom="15pt">
                    <fo:block padding-bottom="5pt" text-align="left"
						font-size="10pt">
                        <xsl:value-of
							select="identAndStatusSection/dmAddress/dmAddressItems/dmTitle/infoName" />
                    </fo:block>
                    <fo:table table-layout="fixed" color="blue"
						padding-bottom="15pt" text-align="left">
                        <fo:table-column column-width="10%" />
                        <fo:table-column column-width="90%" />
                        <fo:table-body>
                            <xsl:for-each select="//proceduralStep/title">
                                <fo:table-row>
                                    <fo:table-cell>
                                        <fo:block>
                                            <xsl:number value="position()" format="1" />
                                        </fo:block>
                                    </fo:table-cell>
                                    <fo:table-cell>
                                        <fo:block text-align-last="justify" font-size="10pt">
                                            <fo:basic-link
												internal-destination="{generate-id(..)}">
                                                <xsl:value-of select="." />
                                                <xsl:if test="graphic">
                                                    <xsl:value-of
														select="mycode:setFigs(number(substring(@id,5)), count(preceding::proceduralStep) + 1) " />
                                                </xsl:if>
                                                <fo:leader leader-pattern="dots" />
                                                <fo:page-number-citation
													ref-id="{generate-id(..)}" />
                                            </fo:basic-link>
                                        </fo:block>
                                    </fo:table-cell>
                                </fo:table-row>
                            </xsl:for-each>
                        </fo:table-body>
                    </fo:table>
                </fo:block>
            </xsl:if>
        </fo:block>
    </xsl:template>
    <!-- Content inside Procedure, Descript, IPD and Fault scheema starts here  -->
    <xsl:template match="content">
        <!-- IPD Chapter starts here -->
        <xsl:if test="illustratedPartsCatalog">
			<xsl:apply-templates
				select="illustratedPartsCatalog" />
			<fo:block padding-top="12mm" text-align="center">
				<xsl:attribute name="font-size">
					<xsl:value-of
					select="$SIZE" />
				</xsl:attribute>
				*** End of Data Module ***
			</fo:block>
		</xsl:if>
        <!-- IPD Chapter starts here -->
        <!-- Descript Chapter starts here -->
     
		  <xsl:if test="description">
			<fo:block xsl:use-attribute-sets="h2" text-align="center"
				padding-bottom="5pt" break-before="odd-page">
				

				
			</fo:block>
			
			
			
			<xsl:apply-templates select="description" />
			<xsl:apply-templates select="//levelledPara" />
			<fo:block padding-top="12mm" text-align="center">
				<xsl:attribute name="font-size">
					<xsl:value-of
					select="$SIZE" />
				</xsl:attribute>
				*** End of Data Module ***
			</fo:block>
		</xsl:if>
        <!-- Descript Chapter ends here -->
        <!-- Proceed Chapter starts here -->
	    <xsl:if test="procedure">
			<fo:block xsl:use-attribute-sets="h3" font-style="italic"
				text-align="center" padding-bottom="5pt" break-before="odd-page">
				
			</fo:block>
			
			
			<xsl:apply-templates select="//proceduralStep" />
			<fo:block padding-top="12mm" text-align="center">
				<xsl:attribute name="font-size">
					<xsl:value-of
					select="$SIZE" />
				</xsl:attribute>
				*** End of Data Module ***
			</fo:block>
		</xsl:if>
        <!-- Proceed Chapter ends here -->
        <!-- faultIsolation Chapter starts here -->
        <xsl:if test="faultIsolation">
			<fo:block 
				 break-before="odd-page" >
		
			</fo:block>
			
			<xsl:apply-templates
				select="//isolationMainProcedure" />
			<fo:block  text-align="center">
				<xsl:attribute name="font-size">
					<xsl:value-of select="$SIZE" />
				</xsl:attribute>
				*** End of Data Module ***
			</fo:block>
		</xsl:if>
        <!-- faultIsolation Chapter ends here -->
        <!-- faultReporting Chapter starts here -->
        <xsl:if test="faultReporting">
			<fo:block xsl:use-attribute-sets="h3" font-style="bold"
				text-align="center" break-before="odd-page" padding-bottom="5pt">
				Fault
				Reporting
			</fo:block>
			<xsl:apply-templates select="//faultReporting" />
			<fo:block padding-top="12mm" text-align="center">
				<xsl:attribute name="font-size">
					<xsl:value-of select="$SIZE" />
				</xsl:attribute>
				*** End of Data Module ***
			</fo:block>
		</xsl:if>
        <!-- faultReporting Chapter ends here -->
    </xsl:template>
    <!-- Content inside Procedure, Descript, IPD and Fault scheema starts here  -->
	
    <!-- Descript chapter starts here -->
   <xsl:template match="description">
		<fo:block padding-top="5pt" padding-bottom="5pt"
			text-align="justify" widows="2" keep-with-previous="auto" orphans="2"
			id="{generate-id(.)}" hyphenate="true" language="en">
			<fo:table>
				<fo:table-column column-width="100%" />
				<fo:table-header>
					<fo:table-cell>
						<fo:block>
							<xsl:choose>
								<xsl:when test="@id">
									<fo:block id="{@id}" />
								</xsl:when>
								<xsl:otherwise></xsl:otherwise>
							</xsl:choose>
						</fo:block>
					</fo:table-cell>
				</fo:table-header>
			 <fo:table-body start-indent="0pt">
                <fo:table-row>
                    <fo:table-cell>
                        <fo:block>
                            <xsl:if test="title">
                                <fo:block font-weight="bold" keep-with-next="always"
                                    space-after="1pt">
                                    <xsl:value-of select="mycode:getFileNum()"/>
                                    <xsl:text>.</xsl:text>
                                    <xsl:number format="1" level="multiple"
                                        count="proceduralStep" />
										<fo:leader leader-pattern="space" leader-pattern-width="12pt" leader-length="0.3cm" />
                                    <xsl:value-of select="title" />
                                </fo:block>
                            </xsl:if>
                            <xsl:apply-templates
                                select="node()[not(self::proceduralStep)]" />
                        </fo:block>
                    </fo:table-cell>
                </fo:table-row>
            </fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>
	<xsl:template match="levelledPara">
		<fo:block padding-top="5pt" padding-bottom="5pt"
			text-align="justify" widows="2" keep-with-previous="auto" orphans="2"
			id="{generate-id(.)}" hyphenate="true" language="en">
			<fo:table>
				<fo:table-column column-width="100%" />
				<fo:table-header>
					<fo:table-cell>
						<fo:block>
							<xsl:choose>
								<xsl:when test="@id">
									<fo:block id="{@id}" />
								</xsl:when>
								<xsl:otherwise></xsl:otherwise>
							</xsl:choose>
						</fo:block>
					</fo:table-cell>
				</fo:table-header>
				<fo:table-body>
					<fo:table-row>
						<fo:table-cell>	
							<fo:block >			
							<xsl:if test="title">
							
							<fo:block font-weight="bold" keep-with-next="always" space-after="3pt">
							<xsl:value-of select="mycode:getFileNum()" />.
										<xsl:number format="1 " level="multiple" />
									<fo:leader leader-pattern="space" leader-pattern-width="8pt" leader-length="0.15cm" />
										<xsl:value-of select="title" />
							</fo:block>
								</xsl:if>
								<xsl:apply-templates
									select="node()[not(self::levelledPara)]" />		
							</fo:block>	
						</fo:table-cell>
					</fo:table-row>
				</fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>
	
	
	
    <xsl:template match="para">
        <xsl:if test="preceding-sibling::para">
            <fo:block>
                <xsl:text></xsl:text>
            </fo:block>
        </xsl:if>
        <xsl:apply-templates
			select="@*|node()[not(self::para)]" />
    </xsl:template>
    <!-- Descript chapter ends here -->
    <!-- Internal Reference for Procedure/Descript schema starts here  -->
    <xsl:template match="internalRef">
        <fo:inline color="blue">
		<xsl:variable name="id" select="concat(name(parent::node()), @id)" />
                <xsl:if test="graphic/hotspot">
                    <xsl:for-each select="graphic/hotspot">
                        <xsl:value-of
							select="mycode:setHots(number(substring(@applicationStructureIdent,4)), $id)" />
                    </xsl:for-each>
                </xsl:if>
            <xsl:choose>
                <!-- Hotspot Reference starts here -->
                <xsl:when test="@internalRefTargetType = 'hotspot'">
                    <xsl:variable name="hid" select="@internalRefId" />
                    <xsl:variable name="phid"
						select="//figure[descendant::hotspot[@id = $hid]]/@id" />
                    <xsl:if test="name(parent::node()) != 'para'">
                        <fo:basic-link internal-destination="{$phid}">
                            <xsl:for-each select="//hotspot[@id = $hid]">
                                <xsl:value-of select="@applicationStructureName" />
                            </xsl:for-each>
                        </fo:basic-link>
                    </xsl:if>
                    <xsl:if test="name(parent::node()) = 'para'">
                        <fo:basic-link internal-destination="{$phid}">
                            <xsl:for-each select="//hotspot[@id = $hid]">
                                <xsl:value-of select="@applicationStructureName" />
                            </xsl:for-each>
                        </fo:basic-link>
                    </xsl:if>
                </xsl:when>
                <!-- Hotspot Reference ends here -->
                <!-- Table Reference starts here -->
                <xsl:when test="@internalRefTargetType = 'table'">
                    <xsl:variable name="tid" select="@internalRefId" />
                    <xsl:variable name="tref" select="//table[@id = $tid]" />
                    <fo:basic-link internal-destination="{$tid}">
						Table

						
                        
                        
                        
                        <xsl:value-of
							select="count(//dmodule//table[@id=$tid]/preceding::table)+1" />
                    </fo:basic-link>
                </xsl:when>
                <!-- Table Reference ends here -->
                <!-- Figure Reference starts here -->
                <xsl:when test="@internalRefTargetType = 'figure'">
                    <xsl:variable name="figId" select="@internalRefId" />
                    <fo:basic-link
						internal-destination="{@internalRefId}">
						Fig

						
                        
                        
                        
                        <xsl:value-of
							select="count(//dmodule//figure[@id=$figId]/preceding::figure)+1" />
                    </fo:basic-link>
                </xsl:when>
                <!-- Figure Reference ends here -->
                <!-- Para Reference starts here -->
                <xsl:when test="@internalRefTargetType = 'para'">
                    <xsl:variable name="paraid" select="@internalRefId" />
                    <xsl:for-each select="//levelledPara[@id = $paraid]">
                        <xsl:variable name="ppara">
                            <xsl:number format="1 " level="multiple"
							
								count="levelledPara" />
                        </xsl:variable>
                        <fo:basic-link internal-destination="{$paraid}">
							Para
							
                            
                            
                            
                            <xsl:value-of select="$ppara" />
                        </fo:basic-link>
                    </xsl:for-each>
                </xsl:when>
                <!-- Para Reference ends here -->
                <!-- Step Reference starts here -->
                <xsl:when test="@internalRefTargetType = 'step'">
                    <xsl:variable name="stepid" select="@internalRefId" />
                    <xsl:for-each select="//proceduralStep[@id = $stepid]">
                        <xsl:variable name="lpara">
                            <xsl:number format="1 " level="multiple"
								count="proceduralStep" />
                        </xsl:variable>
                        <fo:basic-link internal-destination="{$stepid}">
							Step

							
                            
                            
                            
                            <xsl:value-of select="$lpara" />
                        </fo:basic-link>
                    </xsl:for-each>
                </xsl:when>
                <!-- Step Reference ends here -->
                <!-- Figure Reference starts here -->
                <xsl:when test="substring(@internalRefId,1,3)='Fig'">
                    <xsl:if test="name(../../..) = 'description'">
                        <xsl:variable name="figId" select="@internalRefId" />
                        <fo:basic-link
							internal-destination="{@internalRefId}">
							Fig

							
                            
                            
                            
                            <xsl:value-of
								select="count(//dmodule//figure[@id=$figId]/preceding::figure)+1" />
                        </fo:basic-link>
                    </xsl:if>
                    <xsl:if test="count(preceding::proceduralStep) > 0">
                        <xsl:variable name="figId" select="@internalRefId" />
                        <fo:basic-link
							internal-destination="{@internalRefId}">
							Fig

							
                            
                            
                            
                            <xsl:value-of
								select="count(//dmodule//figure[@id=$figId]/preceding::figure)+1" />
                        </fo:basic-link>
                    </xsl:if>
                </xsl:when>
                <!-- Figure Reference ends here -->
            </xsl:choose>
        </fo:inline>
        <!-- Support Equipment, Supply and Spare Reference IDs starts here -->
        <fo:inline>
            <xsl:choose>
                <xsl:when test="substring(@internalRefId,1,3)!='Fig'">
                    <xsl:variable name="superid" select="@internalRefId" />
                    <xsl:apply-templates
						select="//supportEquipDescr[@id=$superid]|//supplyDescr[@id=$superid]|//spareDescr[@id=$superid]" />
                </xsl:when>
            </xsl:choose>
        </fo:inline>
        <!-- Support Equipment, Supply and Spare Reference IDs ends here -->
    </xsl:template>
    <!-- Internal Reference for Procedure/Descript schema ends here  -->
    <!-- DM Reference starts here -->
    <xsl:template match="dmRef">
        <fo:inline color="blue">
            <xsl:for-each select="dmRefIdent/dmCode">
                <xsl:variable name="refdm">
                    <xsl:value-of select="@modelIdentCode" />-
                    
                    
                    
                    <xsl:value-of select="@systemDiffCode" />-
                    
                    
                    
                    <xsl:value-of select="@systemCode" />-
                    
                    
                    
                    <xsl:value-of select="@subSystemCode" />
                    <xsl:value-of select="@subSubSystemCode" />-
                    
                    
                    
                    <xsl:value-of select="@assyCode" />-
                    
                    
                    
                    <xsl:value-of select="@disassyCode" />
                    <xsl:value-of select="@disassyCodeVariant" />-
                    
                    
                    
                    <xsl:value-of select="@infoCode" />
                    <xsl:value-of select="@infoCodeVariant" />-
                    
                    
                    
                    <xsl:value-of select="@itemLocationCode" />
                </xsl:variable>
                <fo:basic-link internal-destination="{$refdm}">
                    <xsl:value-of select="$refdm" />
                </fo:basic-link>
            </xsl:for-each>
        </fo:inline>
    </xsl:template>
    <!-- DM Reference ends here -->
    <!-- Support Equipment, Supply and Spare Reference starts here -->
    <xsl:template
		match="//supportEquipDescr|//supplyDescr|//spareDescr">
        <fo:inline color="blue">
            <fo:basic-link internal-destination="{@id}">
                <xsl:value-of select="name" />
            </fo:basic-link>
        </fo:inline>
    </xsl:template>
    <!-- Support Equipment, Supply and Spare Reference ends here -->
    <xsl:template match="title"></xsl:template>
    <!-- Random list element starts here  -->
    <xsl:template match="randomList">
        <xsl:value-of select="mycode:setRandomList(10)" />
        <xsl:if test="name(parent::node()) = 'entry'">
            <xsl:value-of select="mycode:setRandomList(0)" />
        </xsl:if>
        <fo:block padding-top="{mycode:getRandomList()}pt">
            <fo:table border-width="1pt">
			
			<fo:table-column column-width="4%"/>
              <fo:table-column column-width="96%"/>
			
                <fo:table-body>
                    <xsl:for-each select="listItem">
                        <fo:table-row padding-top="3pt">
                            <fo:table-cell>
                                <fo:block >
									• <!-- Bullet point -->
								  </fo:block>
                            </fo:table-cell>
							
							<fo:table-cell>
                                <fo:block  text-align="justify">
									 
                                    <xsl:value-of select="para" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                    </xsl:for-each>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
    <fo:list-item>
        <fo:list-item-label start-indent="1.0cm"
			end-indent="1.0cm">
            <fo:block>
                <xsl:value-of select="item" />
            </fo:block>
        </fo:list-item-label>
        <fo:list-item-body start-indent="body-start()">
            <fo:block>
                <xsl:value-of select="para" />
            </fo:block>
        </fo:list-item-body>
    </fo:list-item>
    <!-- Random list element starts here  -->
    <!-- Sequence list element starts here  -->
    <xsl:template match="sequentialList">
        <xsl:value-of select="mycode:setRandomList(10)" />
        <xsl:if test="name(parent::node()) = 'entry'">
            <xsl:value-of select="mycode:setRandomList(0)" />
        </xsl:if>
        <fo:block padding-top="{mycode:getRandomList()}pt">
            <fo:table border-width="1pt">
                <fo:table-body>
                    <xsl:for-each select="listItem">
                        <fo:table-row padding-top="10pt">
                            <fo:table-cell>
                                <fo:block padding-bottom="10pt">
									➤ 
									
                                    
                                    
                                    
                                    <fo:leader leader-pattern="space"
										leader-pattern-width="8pt" leader-length="0.5cm" />
                                    <xsl:value-of select="para" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                    </xsl:for-each>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
    <!-- Sequence list element ends here  -->
    <!-- Multimedia ( Insertion of graphic) starts here  -->
    <xsl:template match="multimedia|figure">
        <xsl:if test="multimediaObject">
            <fo:block text-align="center" id="{generate-id(.)}" />
        </xsl:if>
        <xsl:if test="graphic">
            <fo:block text-align="center" id="{generate-id(.)}" page-break-inside="avoid" padding-top="7pt">
                <fo:external-graphic content-height="8.00in"
					width="100%" content-width="scale-to-fit" scaling="uniform">
                    <xsl:attribute name="src">
                        <xsl:value-of
						select="concat($PATH, concat(graphic/@infoEntityIdent,'.cgm'))" />
                    </xsl:attribute>
                </fo:external-graphic>
                <xsl:variable name="id" select="concat(name(parent::node()), @id)" />
                <xsl:if test="graphic/hotspot">
                    <xsl:for-each select="graphic/hotspot">
                        <xsl:value-of
							select="mycode:setHots(number(substring(@applicationStructureIdent,4)), $id)" />
                    </xsl:for-each>
                </xsl:if>
                <!--  Figure title starts here  -->
                <fo:block page-break-inside="avoid" padding-top="5pt"
					padding-bottom="5pt" font-weight="bold" text-align="center" font-size="10pt"
					id="{@id}">
					Figure 
					
                    
                    <xsl:value-of select="mycode:getFileNum()" />.
                    
                    <xsl:value-of select="count(preceding::graphic) + count(preceding::multimedia) + 1" />:
					
                    
                    
                    
                    <fo:leader leader-pattern="space"
						leader-pattern-width="8pt" leader-length="0.1cm" />
                    <xsl:value-of select="title" />
                </fo:block>
                <!--  Figure title ends here  -->
            </fo:block>
        </xsl:if>
    </xsl:template>
    <!-- Multimedia ( Insertion of graphic) ends here  -->
    <!-- Table starts here  -->
    <xsl:template match="table">
        <fo:block id="{@id}" padding-bottom="5pt" ></fo:block>
        <xsl:apply-templates select="tgroup" />
        <fo:block padding-top="5pt" text-align="center" font-size="10pt" 
			font-weight="bold" id="{generate-id(.)}">
			Table 
           <xsl:value-of select="mycode:getFileNum()" />.
            <xsl:value-of select="count(preceding::table) + 1" />: 
        
            <fo:leader leader-pattern="space"
				leader-pattern-width="8pt" leader-length="0.1cm" />
            <xsl:value-of select="title" />
        </fo:block>
    </xsl:template>
    <xsl:template match="tgroup">
        <fo:table font-size="10pt" inline-progression-dimension="auto" page-break-before="avoid">
            <fo:table-header >
                <fo:table-row height="3mm" border="0pt solid black" font-weight="bold" font-size="10pt"
					display-align="before" text-align="start">
                    <fo:table-cell number-columns-spanned="any"
						font-size="12pt">
                        <fo:block margin-top="3mm">
                            <fo:retrieve-table-marker
								retrieve-class-name="continued"
								retrieve-position-within-table="first-starting"
								retrieve-boundary-within-table="table-fragment" />
                        </fo:block>
                    </fo:table-cell>
                </fo:table-row>
                <xsl:for-each select="thead/row">
                    <fo:table-row  background-color="#D8D8D8">
                        <xsl:for-each select="entry">
                            <fo:table-cell border="1pt solid black" padding="2mm"
								text-align="{@align}"
								number-columns-spanned="{substring(@nameend,4)-substring(@namest,4)}+1">
                                <fo:block font-weight="bold" padding="5pt" wrap-option="wrap">
                                    <!--<xsl:variable name="colspan" select="@internalRefId"/> -->
                                    <xsl:value-of select="para" />
                                </fo:block>
                            </fo:table-cell>
                        </xsl:for-each>
                    </fo:table-row>
                </xsl:for-each>
            </fo:table-header>
            <fo:table-body>
                <xsl:for-each select="tbody/row">
                    <fo:table-row>
                        <xsl:for-each select="entry">
                            <xsl:choose>
                                <xsl:when test="self::node()[@nameend] | self::node()[@morerows]">
                                    <fo:table-cell padding="2mm"
										border="1pt solid black" text-align="{@align}"
										number-columns-spanned="{substring(@nameend,4)-substring(@namest,4)}+1"
										number-rows-spanned="{@morerows}+1" font-size="8pt">
                                        <fo:block padding-top="3pt" padding-bottom="3pt"
											wrap-option="wrap" hyphenate="true" language="en" font-size="8pt">
                                            <xsl:apply-templates select="node()" />
                                        </fo:block>
                                    </fo:table-cell>
                                </xsl:when>
                                <xsl:otherwise>
                                    <fo:table-cell padding="2mm"
										border="1pt solid black" text-align="{@align}">
                                        <fo:block padding-top="5pt" padding-end="5pt" font-size="10pt">
                                            <xsl:apply-templates select="node()" />
                                        </fo:block>
                                    </fo:table-cell>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:for-each>
                        <xsl:for-each select="para">
                            <xsl:choose>
                                <xsl:when test="self::node()[@nameend] | self::node()[@morerows]">
                                    <fo:table-cell padding="2mm"
										border="1pt solid black" text-align="{@align}"
										number-columns-spanned="{substring(@nameend,4)-substring(@namest,4)}+1"
										number-rows-spanned="{@morerows}+1" font-size="8pt">
                                        <fo:block padding-top="3pt" padding-bottom="3pt"
											wrap-option="wrap" hyphenate="true" language="en" font-size="8pt">
                                            <xsl:apply-templates select="node()" />
                                        </fo:block>
                                    </fo:table-cell>
                                </xsl:when>
                                <xsl:otherwise>
                                    <fo:table-cell padding="2mm"
										border="1pt solid black" text-align="{@align}">
                                        <fo:block padding-top="5pt" padding-end="5pt" font-size="10pt">
                                            <xsl:apply-templates select="node()" />
                                        </fo:block>
                                    </fo:table-cell>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:for-each>
                    </fo:table-row>
                </xsl:for-each>
            </fo:table-body>
        </fo:table>
    </xsl:template>
    <xsl:template match="entry[@nameend] | entry[@morerows]">
        <xsl:value-of select="concat(position(), ') ')" />
        <xsl:value-of
			select="/clause/variable[@col='1' and @row=current()/@row]" />
    </xsl:template>
    <!--  Table ends here  -->
	<!-- PreliminaryRqmts starts here -->
	<xsl:template match="preliminaryRqmts">
		<fo:block xsl:use-attribute-sets="h3" text-align="left">
			Required
			Personnel
		</fo:block>
		<fo:block hyphenate="true" language="en">
			<fo:block padding-bottom="5pt" padding-top="10pt"
				text-align="center" font-style="italic" font-size="10pt">
				Table 1
				Requirement Personnel
			</fo:block>
			<fo:table border-top="0.1mm solid"
				border-bottom="0.1mm solid" text-align="left" font-size="10pt">
				<fo:table-column column-width="100pt" />
				<fo:table-column column-width="100pt" />
				<fo:table-column column-width="100pt" />
				<fo:table-column column-width="100pt" />
				<fo:table-column column-width="100pt" />
				<fo:table-header>
					<fo:table-row>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Person</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Category/Trade</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Skill Level</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Trade Code</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Estimated Time</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-header>
				<fo:table-body>
					<xsl:for-each select="reqPersons">
						<xsl:for-each select="person">
							<fo:table-row>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										Man

										<xsl:value-of select="@man" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of
											select="personCategory/@personCategoryCode" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt" />
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of select="trade" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of select="estimatedTime" />
										<xsl:value-of
											select="estimatedTime/@unitOfMeasure" />
									</fo:block>
								</fo:table-cell>
							</fo:table-row>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:if test="not(reqPersons)">
						<fo:table-row>
							<fo:table-cell>
								<fo:block padding-top="5pt">None </fo:block>
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
				</fo:table-body>
			</fo:table>
		</fo:block>

		<fo:block xsl:use-attribute-sets="h3" text-align="left">
			Support
			Equipment
		</fo:block>
		<fo:block page-break-inside="avoid" hyphenate="true"
			language="en">
			<fo:block padding-bottom="5pt" padding-top="10pt"
				text-align="center" font-style="italic" font-size="10pt">
				Table 2 Support
				Equipment
			</fo:block>
			<fo:table border-top="0.1mm solid"
				border-bottom="0.1mm solid" font-size="10pt" text-align="left">
				<fo:table-column column-width="180pt" />
				<fo:table-column column-width="200pt" />
				<fo:table-column column-width="60pt" />
				<fo:table-column column-width="60pt" />
				<fo:table-header>
					<fo:table-row>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Nomenclature</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Identification No.</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Qty</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Remarks</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-header>
				<fo:table-body>
					<xsl:for-each
						select="reqSupportEquips/supportEquipDescrGroup">
						<xsl:for-each select="supportEquipDescr">
							<fo:table-row>
								<fo:table-cell>
									<fo:block padding-top="5pt" id="{@id}">
										<xsl:value-of select="name" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										NSCM:

										<xsl:value-of
											select="identNumber/manufacturerCode" />
									</fo:block>
									<fo:block padding-top="5pt">
										Pt. No.:

										<xsl:value-of
											select="identNumber/partAndSerialNumber/partNumber" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of select="reqQuantity" />
										<xsl:value-of select="reqQuantity/@unitOfMeasure" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt" />
								</fo:table-cell>
							</fo:table-row>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:if test="reqSupportEquips/noSupportEquips">
						<fo:table-row>
							<fo:table-cell>
								<fo:block padding-top="5pt">None </fo:block>
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
				</fo:table-body>
			</fo:table>
		</fo:block>

		<fo:block xsl:use-attribute-sets="h3" text-align="left">
			Consumables, Materials and Expendables
		</fo:block>
		<fo:block hyphenate="true" language="en">
			<fo:block padding-bottom="5pt" padding-top="10pt"
				text-align="center" font-style="italic" font-size="10pt">
				Table 3 Supplies
			</fo:block>
			<fo:table border-top="0.1mm solid"
				border-bottom="0.1mm solid" font-size="10pt" text-align="left">
				<fo:table-column column-width="180pt" />
				<fo:table-column column-width="180pt" />
				<fo:table-column column-width="80pt" />
				<fo:table-column column-width="60pt" />
				<fo:table-header>
					<fo:table-row>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Nomenclature</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Identification No.</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Qty</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Remarks</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-header>
				<fo:table-body>
					<xsl:for-each select="reqSupplies/supplyDescrGroup">
						<xsl:for-each select="supplyDescr">
							<fo:table-row>
								<fo:table-cell>
									<fo:block padding-top="5pt" id="{@id}">
										<xsl:value-of select="name" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										NSCM:

										<xsl:value-of
											select="identNumber/manufacturerCode" />
									</fo:block>
									<fo:block padding-top="5pt">
										Pt. No.:

										<xsl:value-of
											select="identNumber/partAndSerialNumber/partNumber" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of select="reqQuantity" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt" />
								</fo:table-cell>
							</fo:table-row>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:if test="reqSupplies/noSupplies">
						<fo:table-row>
							<fo:table-cell>
								<fo:block padding-top="5pt">None </fo:block>
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
				</fo:table-body>
			</fo:table>
		</fo:block>

		<fo:block xsl:use-attribute-sets="h3" text-align="left">
			Replacement
			Parts/Items
		</fo:block>
		<fo:block hyphenate="true" language="en">
			<fo:block padding-bottom="5pt" padding-top="10pt"
				text-align="center" font-style="italic" font-size="10pt">
				Table 4 Spares
			</fo:block>
			<fo:table border-top="0.1mm solid"
				border-bottom="0.1mm solid" font-size="10pt" text-align="left">
				<fo:table-column column-width="180pt" />
				<fo:table-column column-width="180pt" />
				<fo:table-column column-width="80pt" />
				<fo:table-column column-width="60pt" />
				<fo:table-header>
					<fo:table-row>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Nomenclature</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Identification No.</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Qty</fo:block>
						</fo:table-cell>
						<fo:table-cell border-top="0.1mm solid"
							border-bottom="0.1mm solid">
							<fo:block font-weight="bold" padding-top="5pt"
								padding-bottom="5pt">Remarks</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-header>
				<fo:table-body>
					<xsl:for-each select="reqSpares/spareDescrGroup">
						<xsl:for-each select="spareDescr">
							<fo:table-row>
								<fo:table-cell>
									<fo:block padding-top="5pt" id="{@id}">
										<xsl:value-of select="name" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										NSCM:

										<xsl:value-of
											select="identNumber/manufacturerCode" />
									</fo:block>
									<fo:block padding-top="5pt">
										Pt. No.:

										<xsl:value-of
											select="identNumber/partAndSerialNumber/partNumber" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt">
										<xsl:value-of select="reqQuantity" />
									</fo:block>
								</fo:table-cell>
								<fo:table-cell>
									<fo:block padding-top="5pt" />
								</fo:table-cell>
							</fo:table-row>
						</xsl:for-each>
					</xsl:for-each>
					<xsl:if test="reqSpares/noSpares">
						<fo:table-row>
							<fo:table-cell>
								<fo:block padding-top="5pt">None </fo:block>
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
							<fo:table-cell>
								<fo:block padding-top="5pt" />
							</fo:table-cell>
						</fo:table-row>
					</xsl:if>
				</fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>
	<!-- PreliminaryRqmts ends here -->
    <!--  Procedure Chapter starts here  -->
  <xsl:template match="proceduralStep">
		<fo:block padding-top="5pt" padding-bottom="5pt"
			text-align="justify" widows="2" keep-with-previous="auto" orphans="2"
			id="{generate-id(.)}" hyphenate="true" language="en">
			<fo:table>
				<fo:table-column column-width="100%" />
				<fo:table-header>
					<fo:table-cell>
						<fo:block>
							<xsl:choose>
								<xsl:when test="@id">
									<fo:block id="{@id}" />
								</xsl:when>
								<xsl:otherwise></xsl:otherwise>
							</xsl:choose>
						</fo:block>
					</fo:table-cell>
				</fo:table-header>
				<fo:table-body start-indent="0pt">
					<fo:table-row>
						<fo:table-cell>
							<fo:block>
								<xsl:if test="title">
									<fo:block font-weight="bold" keep-with-next="always"
										space-after="5pt">
										<xsl:value-of select="mycode:getFileNum()" />.
										<xsl:number format="1 " level="multiple"
									count="proceduralStep" />
									<fo:leader leader-pattern="space" leader-pattern-width="8pt" leader-length="0.3cm" />
										<xsl:value-of select="title" />
									</fo:block>
								</xsl:if>
								<xsl:apply-templates
									select="node()[not(self::proceduralStep)]" />
							</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>
	
    <!--  Procedure Chapter ends here  -->
    <!-- Caution element starts here  -->
    <xsl:template match="caution">
        <!--   Added caution logo, Defined the size of logo, border thickness and colour    -->
        <fo:block font-size="10pt" font-weight="bold" color="black" margin-bottom="5pt">
            <fo:table border="1px solid #FFA500" border-color="#FFA500"  padding-bottom="2pt" width="100%">
                <fo:table-column column-width="13%"/>
                <fo:table-column column-width="87%"/>
                <fo:table-header>
                    <fo:table-row>
                        <fo:table-cell>
                            <fo:block />
                        </fo:table-cell>
                        <fo:table-cell border-top="2px #FFA500" display-align="after">
                            <fo:block color="#FFA500" font-size="10pt" font-weight="bold" padding-top="7pt" text-align="center">
            CAUTION
          </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body>
                    <fo:table-row>
                        <fo:table-cell  font-family="Tahoma" >
                            <fo:block text-align="center">
                                <fo:external-graphic src="caution.png" width="50%" content-width="scale-to-fit"/>
                            </fo:block>
                        </fo:table-cell>
                        <fo:table-cell >
                            <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="7pt"
							padding-bottom="7pt">
                                <xsl:for-each select="warningAndCautionPara"/>
                                <xsl:apply-templates select="@*|node()" />
                            </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
    <!-- Caution element ends here  -->
    <!-- Warning element starts here  -->
    <xsl:template match="warning">
        <!--   Added warning logo, Defined the size of logo, border thickness and colour    -->
        <fo:block font-size="10pt" font-weight="bold" color="black" margin-bottom="5pt">
            <fo:table border="1px solid red" border-color="red"  padding-bottom="2pt" width="100%">
                <fo:table-column column-width="13%"/>
                <fo:table-column column-width="87%"/>
                <fo:table-header>
                    <fo:table-row>
                        <fo:table-cell>
                            <fo:block/>
                        </fo:table-cell>
                        <fo:table-cell border-top="2px red" display-align="after">
                            <fo:block color="red" font-size="10pt" font-weight="bold" padding-top="7pt" text-align="center">
            WARNING
          </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body>
                    <fo:table-row>
                        <fo:table-cell  font-family="Tahoma" >
                            <fo:block text-align="center">
                                <fo:external-graphic src="Warning.png" width="50%" content-width="scale-to-fit"/>
                            </fo:block>
                        </fo:table-cell>
                        <fo:table-cell >
                            <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="7pt"
							padding-bottom="7pt">
                                <xsl:for-each select="warningAndCautionPara"/>
                                <xsl:apply-templates select="@*|node()" />
                            </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
	
	<xsl:template match="warningAndCautionPara">
  <xsl:if test="position() = 1">
    <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="5pt"
							padding-bottom="7pt">
      
      <xsl:apply-templates/>
     
    </fo:block>
  </xsl:if>
  <xsl:if test="position() &gt; 1">
     <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="5pt"
							padding-bottom="7pt">
      <xsl:apply-templates/>
     
    </fo:block>
  </xsl:if>
 </xsl:template>
	
    <!-- Warning element ends here  -->
    <!-- Note element starts here  -->
    <xsl:template match="note">
        <!--   Added Note logo, Defined the size of logo, border thickness and colour    -->
        <fo:block font-size="10pt" font-weight="bold" color="black" margin-bottom="5pt">
            <fo:table border="1px solid black" border-color="black"  padding-bottom="2pt" width="100%">
                <fo:table-column column-width="13%"/>
                <fo:table-column column-width="87%"/>
                <fo:table-header>
                    <fo:table-row>
                        <fo:table-cell>
                            <fo:block/>
                        </fo:table-cell>
                        <fo:table-cell border-top="2px black" display-align="after">
                            <fo:block color="black" font-size="10pt" font-weight="bold" padding-top="7pt" text-align="center">
            NOTE
          </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body>
                    <fo:table-row>
                        <fo:table-cell  font-family="Tahoma" >
                            <fo:block text-align="center">
                                <fo:external-graphic src="Note.png" width="50%" content-width="scale-to-fit"/>
                            </fo:block>
                        </fo:table-cell>
                        <fo:table-cell >
                            <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="7pt"
							padding-bottom="7pt">
                                <xsl:for-each select="notePara"/>
								<xsl:apply-templates select="@*|node()" />
								
                           </fo:block>
					
                        </fo:table-cell>
						
                    </fo:table-row>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
	
	  <xsl:template match="notePara">
  <xsl:if test="position() = 1">
    <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="5pt"
							padding-bottom="7pt">
      
      <xsl:apply-templates/>
     
    </fo:block>
  </xsl:if>
  <xsl:if test="position() &gt; 1">
     <fo:block text-align="left" color="black"
							font-weight="bold" font-size="10pt" padding-top="5pt"
							padding-bottom="7pt">
      <xsl:apply-templates/>
     
    </fo:block>
  </xsl:if>
 </xsl:template>
    <!-- Note element ends here  -->
    <!-- IPD chapter starts here  -->
    <xsl:template match="illustratedPartsCatalog">
        <!--   Modified all illustrated parts catalog format    -->
        <fo:block page-break-before="always" />
        <xsl:apply-templates select="figure" />
        <fo:block page-break-before="always" hyphenate="true"
			language="en">
            <fo:table border="1pt solid black" >
                <fo:table-column column-width="50pt" />
                <fo:table-column column-width="90pt" />
                <fo:table-column column-width="20pt" />
                <fo:table-column column-width="65pt" />
                <fo:table-column column-width="170pt" />
                <fo:table-column column-width="70pt" />
                <fo:table-column column-width="30pt" />
                <fo:table-header border="1pt solid black">
                    <fo:table-row background-color="#D8D8D8" border="1pt solid black">
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="5pt" padding-bottom="5pt"  text-align="center">Design No.
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Make
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Ind.
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Part No.
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Description
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Reference
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after"
							border="1pt solid black">
                            <fo:block padding-top="1pt" padding-bottom="10pt"  text-align="center">Qty</fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body border="1pt solid black">
                    <xsl:for-each select="catalogSeqNumber">
                        <xsl:for-each select="itemSequenceNumber">
                            <fo:table-row  border="1pt solid black">
                                <!-- Design No.  Col 1 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="substring(../@catalogSeqNumberValue, 1)" />
                                    </fo:block>
                                </fo:table-cell>
                                <!--Make   Col 2 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="manufacturerCode" />
                                    </fo:block>
                                </fo:table-cell>
                                <!-- Indenture Col 3 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="substring(../@indenture, 1)" />
                                    </fo:block>
                                </fo:table-cell>
                                <!-- Part No. Col 4 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="partNumber" />
                                    </fo:block>
                                </fo:table-cell>
                                <!-- Description Col 5 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of
											select="substring('• • • ', 1, (number(@indenture)-1)*2)" />
                                        <xsl:value-of
											select="partIdentSegment/descrForPart" />
                                    </fo:block>
                                </fo:table-cell>
                                <!-- Ref. Col 6 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="substring(../@reasonForUpdateRefIds, 1)" />
                                    </fo:block>
                                </fo:table-cell>
                                <!-- Quantity Col 7 -->
                                <fo:table-cell border="1pt solid black" text-align="center">
                                    <fo:block padding-top="5pt" padding-bottom="5pt">
                                        <xsl:value-of select="quantityPerNextHigherAssy" />
                                    </fo:block>
                                </fo:table-cell>
                            </fo:table-row>
                        </xsl:for-each>
                    </xsl:for-each>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
    <!-- IPD chapter ends here  -->
    <!--  Fault Isolation Main Procedure chapter starts here  -->
   <xsl:template match="fault">
		<fo:block xsl:use-attribute-sets="h3" font-style="italic"
			text-align="left" padding-bottom="5pt" hyphenate="true" language="en" />
		Fault Code:
		<xsl:value-of select="@faultCode" />
	</xsl:template>
	<xsl:template match="isolationProcedure">
		<fo:block xsl:use-attribute-sets="h3" font-style="italic"
			text-align="center" padding-bottom="5pt">
			Preliminary Requirements
		</fo:block>
		<xsl:if test="preliminaryRqmts" />
		<xsl:apply-templates select="preliminaryRqmts" />
	</xsl:template>
	<xsl:template match="isolationMainProcedure">
		
		<fo:block>
			<xsl:if test="table" />
			<xsl:apply-templates select="table" />
		</fo:block>
		<fo:block>
			<xsl:if test="isolationStep" />
			<xsl:apply-templates
				select="isolationStep" />
		</fo:block>
	</xsl:template>		
	
<!-- Match the isolationStep and build the full table -->
<xsl:template match="isolationStep">
    <fo:block padding-top="10pt" padding-bottom="10pt">
        <fo:table table-layout="fixed" width="100%" border-collapse="collapse">
            <!-- Define 3 columns -->
            <fo:table-column column-width="30%"/>
            <fo:table-column column-width="35%"/>
            <fo:table-column column-width="35%"/>

            <!-- Table Header -->
            <fo:table-header>
                <fo:table-row>
                    <fo:table-cell border="0.5pt solid black" padding="4pt">
                        <fo:block font-weight="bold">Fault</fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.5pt solid black" padding="4pt">
                        <fo:block font-weight="bold">Cause</fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.5pt solid black" padding="4pt">
                        <fo:block font-weight="bold">Remedy</fo:block>
                    </fo:table-cell>
                </fo:table-row>
            </fo:table-header>

            <!-- Table Body -->
            <fo:table-body>
                <xsl:variable name="faultText" select="normalize-space(isolationStepQuestion)"/>

                <xsl:for-each select=".//choice">
                    <xsl:variable name="refid" select="@nextActionRefId"/>
                    <fo:table-row>
                        <!-- Fault shown only once -->
                        <fo:table-cell border="0.5pt solid black" padding="4pt">
                            <fo:block>
                                <xsl:if test="position() = 1">
                                    <xsl:value-of select="$faultText"/>
                                </xsl:if>
                            </fo:block>
                        </fo:table-cell>

                        <!-- Cause -->
                        <fo:table-cell border="0.5pt solid black" padding="4pt">
                            <fo:block>
                                <xsl:apply-templates select="node()"/>
                            </fo:block>
                        </fo:table-cell>

                        <!-- Remedy -->
                        <fo:table-cell border="0.5pt solid black" padding="4pt">
                            <fo:block>
                                <xsl:value-of select="//isolationProcedureEnd[@id = $refid]/action"/>
                            </fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </xsl:for-each>
            </fo:table-body>
        </fo:table>
    </fo:block>
</xsl:template>


	<xsl:template match="yesAnswer">
		<fo:block padding-top="5pt" id="{generate-id(.)}"
			hyphenate="true" language="en">
			<fo:table>
				<fo:table-column column-width="200pt" />
				<fo:table-column column-width="500pt" />
				<fo:table-header>
					<fo:table-cell>
						<fo:block>
							<xsl:choose>
								<xsl:when test="@id">
									<fo:block id="{@id}" />
								</xsl:when>
							</xsl:choose>
						</fo:block>
					</fo:table-cell>
				</fo:table-header>
				<fo:table-body>
					<fo:table-row>
						<fo:table-cell>
							<fo:block>
								<xsl:choose>
									<xsl:when test="@nextActionRefId">
										➤ Yes: Go To

										<xsl:variable name="yesid"
											select="@nextActionRefId" />
										<xsl:for-each
											select="//isolationProcedureEnd[@id = $yesid]">
											<xsl:variable name="ystep">
												<xsl:number format="1" level="multiple"
													count="isolationStep|isolationProcedureEnd" />
											</xsl:variable>
											<fo:basic-link internal-destination="{$yesid}">
												<fo:inline color="blue">
													Step

													<xsl:value-of select="$ystep" />
												</fo:inline>
											</fo:basic-link>
										</xsl:for-each>
										<xsl:for-each
											select="//isolationStep[@id = $yesid]">
											<xsl:variable name="ysstep">
												<xsl:number format="1" level="multiple"
													count="isolationStep|isolationProcedureEnd" />
											</xsl:variable>
											<fo:basic-link internal-destination="{$yesid}">
												<fo:inline color="blue">
													Step

													<xsl:value-of select="$ysstep" />
												</fo:inline>
											</fo:basic-link>
										</xsl:for-each>
										<xsl:apply-templates />
									</xsl:when>
								</xsl:choose>
							</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>

	<xsl:template match="noAnswer">
		<fo:block padding-top="5pt" id="{generate-id(.)}">
			<fo:table>
				<fo:table-column column-width="200pt" />
				<fo:table-column column-width="500pt" />
				<fo:table-header>
					<fo:table-cell>
						<fo:block>
							<xsl:choose>
								<xsl:when test="@id">
									<fo:block id="{@id}" />
								</xsl:when>
							</xsl:choose>
						</fo:block>
					</fo:table-cell>
				</fo:table-header>
				<fo:table-body>
					<fo:table-row>
						<fo:table-cell>
							<fo:block>
								<xsl:choose>
									<xsl:when test="@nextActionRefId">
										➤ No: Go To

										<xsl:variable name="noid"
											select="@nextActionRefId" />
										<xsl:for-each
											select="//isolationStep[@id = $noid]">
											<xsl:variable name="nstep">
												<xsl:number format="1" level="multiple"
													count="isolationStep|isolationProcedureEnd" />
											</xsl:variable>
											<fo:basic-link internal-destination="{$noid}">
												<fo:inline color="blue">
													Step

													<xsl:value-of select="$nstep" />
												</fo:inline>
											</fo:basic-link>
										</xsl:for-each>
										<xsl:for-each
											select="//isolationProcedureEnd[@id = $noid]">
											<xsl:variable name="npstep">
												<xsl:number format="1" level="multiple"
													count="isolationStep|isolationProcedureEnd" />
											</xsl:variable>
											<fo:basic-link internal-destination="{$noid}">
												<fo:inline color="blue">
													Step

													<xsl:value-of select="$npstep" />
												</fo:inline>
											</fo:basic-link>
										</xsl:for-each>
										<xsl:apply-templates />
									</xsl:when>
								</xsl:choose>
							</fo:block>
						</fo:table-cell>
					</fo:table-row>
				</fo:table-body>
			</fo:table>
		</fo:block>
	</xsl:template>
	<!--  No Answer ends here  -->
    <!--  Fault Isolation Main Procedure chapter ends here  -->
    <!--  Fault Reporting chapter starts here  -->
    <xsl:template match="faultReporting">
        <fo:block hyphenate="true" language="en">
            <fo:table>
                <fo:table-column column-width="200pt" />
                <fo:table-column column-width="250pt" />
                <fo:table-header>
                    <fo:table-row font-weight="bold" text-align="justify">
                        <fo:table-cell display-align="after">
                            <fo:block padding-top="5pt" padding-bottom="5pt">Fault ID
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after">
                            <fo:block padding-top="5pt" padding-bottom="5pt">Fault Code
							</fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body text-align="justify">
                    <xsl:for-each select="observedFault">
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of select="@id" />
                                </fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of select="@faultCode" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of select="faultDescr/descr" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/faultContext" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                    </xsl:for-each>
                </fo:table-body>
            </fo:table>
            <fo:table>
                <fo:table-column column-width="150pt" />
                <fo:table-column column-width="250pt" />
                <fo:table-column column-width="100pt" />
                <fo:table-header>
                    <fo:table-row font-weight="bold" text-align="justify">
                        <fo:table-cell display-align="after">
                            <fo:block padding-top="5pt" padding-bottom="5pt">Part
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after">
                            <fo:block padding-top="5pt" padding-bottom="5pt">Manufacturer
							</fo:block>
                        </fo:table-cell>
                        <fo:table-cell display-align="after">
                            <fo:block padding-top="5pt" padding-bottom="5pt">Part Number
							</fo:block>
                        </fo:table-cell>
                    </fo:table-row>
                </fo:table-header>
                <fo:table-body text-align="justify">
                    <xsl:for-each select="observedFault">
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/lru/name" />
                                </fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/lru/identNumber/manufacturerCode" />
                                </fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/lru/identNumber/partAndSerialNumber/partNumber" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> Test Type
								</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/@testType" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> Test Code
								</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/@testCode" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> Description
								</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/testDescr/testName" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> Test
									Parameters </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> From
								</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/testParameters/@from" />
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/testParameters/@unitOfMeasure" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                        <fo:table-row>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt"> To
								</fo:block>
                            </fo:table-cell>
                            <fo:table-cell>
                                <fo:block padding-top="5pt" padding-bottom="5pt">
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/testParameters/@to" />
                                    <xsl:value-of
										select="contextAndIsolationInfo/isolationInfo/lruItem/faultIsolationTest/testParameters/@unitOfMeasure" />
                                </fo:block>
                            </fo:table-cell>
                        </fo:table-row>
                    </xsl:for-each>
                </fo:table-body>
            </fo:table>
        </fo:block>
    </xsl:template>
    <!--  Fault Reporting chapter ends here  -->
    <!--  Acronym element starts here  -->
    <xsl:template match="acronym">
        <fo:inline color="blue">
            <xsl:value-of select="./acronymTerm"/>
        </fo:inline>
    </xsl:template>
	<!--  Acronym element ends here  -->
	<xsl:template match="subScript">
  <fo:inline vertical-align="sub">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>
	
	
<xsl:template match="superScript">
  <fo:inline vertical-align="super">
    <xsl:apply-templates/>
  </fo:inline>
</xsl:template>



  <!-- HTTP link xslt-->

<xsl:template match="externalPubRef">
        <fo:inline text-decoration="underline">
       <xsl:value-of select="@authorityDocument" />
	     </fo:inline>
</xsl:template> 
	
	
</xsl:stylesheet>