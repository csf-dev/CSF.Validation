<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="http://www.w3.org/1999/xhtml">
  <xsl:output method="html" />
  <xsl:template match="/">
<html>
<head>
<title>NUnit failures</title>
<style>
body {
  background: #F7F2E3;
  font-family: "Droid Sans", "DejaVu Sans", Verdana, Arial, sans;
  margin: 0;
  padding: 1em;
}
header {
  padding: 0 0 0.4em;
  margin: 0.4em 0 0.6em;
  border-bottom: 1px solid #777;
  overflow: auto;
}
h1 {
  font-size: normal;
  font-size: 200%;
  font-weight: normal;
  font-family: "DejaVu Serif", "Times New Roman", Georgia, serif;
  padding: 0;
  margin: 0;
  float: left;
}
ul.info {
  float: right;
  list-style: none;
  margin: 0;
  padding: 0;
  font-size: 80%;
  max-width: 22.5em;
}
ul.info li {
  float: left;
  width: 10em;
  margin-right: 1.2em;
}
ul.info li .key {
  display: inline-block;
  min-width: 5em;
  color: #888;
}
ul.failures {
  margin: 0;
  padding: 0;
  list-style: none;
}
ul.failures li {
  margin: 0 0 2em;
}
h2 {
  font-weight: normal;
  font-size: normal;
  font-family: "Droid Sans Mono", "DejaVu Sans Mono", "Lucida Console", "Courier New", Courier, monospace;
  font-size: 90%;
  margin: 0 0 0.2em;
  padding: 0.4em;
}
.test_suite h2 {
  background: #B2E7F4;
}
.test_suite h2 span {
  font-family: "Droid Sans", "DejaVu Sans", Verdana, Arial, sans;
  font-weight: bold;
}
.test_case h2 {
  background: #F7E5B4;
}
p.description {
  margin: 0 0 0.2em;
  font-style: italic;
}
h3 {
  font-size: normal;
  font-size: 105%;
  font-weight: normal;
  margin: 0.4em 0 0.1em;
}
pre {
  margin: 0;
  padding: 0.1em;
}
</style>
</head>
<body>
<header>
  <h1>Test result summary</h1>
  <ul class="info">
    <li>
      <span class="key">Tests</span>
      <span class="value"><xsl:value-of select="/test-results/@total"/></span>
    </li>
    <li>
      <span class="key">Failures</span>
      <span class="value"><xsl:value-of select="/test-results/@failures"/></span>
    </li>
    <li>
      <span class="key">Errors</span>
      <span class="value"><xsl:value-of select="/test-results/@errors"/></span>
    </li>
    <li>
      <span class="key">Skipped</span>
      <span class="value"><xsl:value-of select="/test-results/@not-run"/></span>
    </li>
    <li>
      <span class="key">Time (s)</span>
      <span class="value"><xsl:value-of select="/test-results/test-suite/@time"/></span>
    </li>
  </ul>
</header>
<ul class="failures">
<xsl:apply-templates select="//test-suite[failure]" />
<xsl:apply-templates select="//test-case[failure]" />
</ul>
</body>
</html>
  </xsl:template>
  
  <xsl:template match="//test-suite[failure]">
    <li class="test_suite">
      <h2><span>Suite failure: </span><xsl:value-of select="@name"/>.<xsl:value-of select="@type"/></h2>
      <p class="description"><xsl:value-of select="@description"/></p>
      <h3>Message</h3>
      <pre><xsl:value-of select="failure/message"/></pre>
      <h3>Stack trace</h3>
      <pre><xsl:value-of select="failure/stack-trace"/></pre>
    </li>
  </xsl:template>

  <xsl:template match="//test-case[failure]">
    <li class="test_case">
      <h2><xsl:value-of select="@name"/></h2>
      <p class="description"><xsl:value-of select="@description"/></p>
      <h3>Message</h3>
      <pre><xsl:value-of select="failure/message"/></pre>
      <h3>Stack trace</h3>
      <pre><xsl:value-of select="failure/stack-trace"/></pre>
    </li>
  </xsl:template>
</xsl:stylesheet>