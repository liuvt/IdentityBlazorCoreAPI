//Nút đăng nhập
window.handleCredentialResponse = async (response) => {
  //Xữ lý token trả về
    console.log("Encoded JWT ID token: " + response.credential);
    console.log("Encoded Decoded: " + response);
    localStorage.setItem("googleApp", response.credential);
}

//Load singin-google ở mọi trang
/*
window.onload = async () => {
    google.accounts.id.initialize({
      client_id: "37285241759-fjd9b14rmn8s7f6ah1l5255mimlhfnt7.apps.googleusercontent.com",
      callback: handleCredentialResponse
    });

    google.accounts.id.renderButton(
      document.getElementById("buttonDiv"),
      { theme: "outline", size: "large" }  // customization attributes
    );
    google.accounts.id.prompt(); // also display the One Tap dialog
}*/

//Load singin-google ở trang chỉ định
window.googleSingInOnPage = async () => {
  google.accounts.id.initialize({
    client_id: "37285241759-fjd9b14rmn8s7f6ah1l5255mimlhfnt7.apps.googleusercontent.com",
    callback: handleCredentialResponse,
  });

  google.accounts.id.renderButton(
    document.getElementById("buttonDiv"),
    { theme: "outline", size: "large" }  //customization attributes
  );
  google.accounts.id.prompt(); //also display the One Tap dialog
}
