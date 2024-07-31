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

// Download file: https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-8.0
window.downloadFileFromStream = async (fileName, content) => {
  // Tạo một định dạng file để chuyển dữ liệu về blob
  const file = new File([content], fileName, { type: "application/octet-stream" });
  // Convert to ObjectUrl để chuyển dữ liệu thành blob
  const url = URL.createObjectURL(file);
  // Tạo một element a để tài xuống
  const anchorElement = document.createElement('a');
  anchorElement.href = url;
  anchorElement.download = fileName;
  anchorElement.click();
  anchorElement.target = "_self";
  anchorElement.remove();
  URL.revokeObjectURL(url);
}
