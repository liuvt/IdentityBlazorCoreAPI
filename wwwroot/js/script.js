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