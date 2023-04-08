function transformHtmlMarkUpToText(text) {
  // decode the URL-encoded string to a byte string
  const byteString = decodeURIComponent(text).replace(
    /%([0-9A-F]{2})/g,
    (match, p1) => String.fromCharCode("0x" + p1)
  );

  // decode the byte string using the 'latin1' encoding to get the final string
  let finalString = new TextDecoder("latin1").decode(
    new Uint8Array([...byteString].map((c) => c.charCodeAt(0)))
  );

  // replace the capital letter 'A' with a lowercase 'a' in the final string
  finalString = finalString.replace(/A/g, "a");

  // return the final string
  return finalString;
}

export default transformHtmlMarkUpToText;
