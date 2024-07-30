//SwiperJS: trượt logo brand
window.callSwiperJSLogoBrand = async () => {
  const swiper = new Swiper('.swiperJsBrand', {
    slidesPerView: "auto",
    loop: true,
    centeredSliders: true,
    speed: 2500,
    allowTouchMove: false,
    disableOnInteraction: false,
    autoplay: {
      delay: -10,
    },
  });
}

//SwiperJS: effect Ds ảnh
window.callSwiperJSEffect = async () => {
  var swiper5 = new Swiper(".swiperJsEffect", {
    slidesPerView: "auto",
    loop: true,
    centeredSliders: true,
    speed: 2000,
    allowTouchMove: false,
    disableOnInteraction: false,
    autoplay: {
      delay: 3000,
    },
    grabCursor: false,
    effect: "creative",
    creativeEffect: {
      prev: {
        translate: ["120%", 0, -500],
      },
      next: {
        translate: ["-120%", 0, -500],
      },
    },
  });
}


window.saveAsFile = async (filename, byteBase64) =>{

      // Create the <a> element and click on it
  const a = document.createElement("a");
  document.body.appendChild(a);
  a.href = "data:application/octet-stream;base64," + byteBase64;
  a.download = filename;
  a.target = "_self";
  a.click();
  document.body.removeChild(a);
  // We don't need to keep the object url, let's release the memory
  // On Safari it seems you need to comment this line... (please let me know if you know why)
  URL.revokeObjectURL(exportUrl);
}