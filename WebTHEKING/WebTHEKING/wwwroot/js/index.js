document.addEventListener('DOMContentLoaded', function() {
    
    // ===================================
    // DATABASE SẢN PHẨM (GIẢ LẬP)
    // ===================================
    const allProducts = {
        // ID: {Dữ liệu}
        1: { id: 1, name: 'Iphone 13', price: 12890000, image: 'img/9.jpg', description: 'Hiệu năng mạnh mẽ với chip A15 Bionic, màn hình Super Retina XDR sắc nét và hệ thống camera kép tiên tiến.' },
        2: { id: 2, name: 'Iphone 14 Pro', price: 13790000, image: 'img/19.jpg', description: 'Trải nghiệm Dynamic Island độc đáo, camera chính 48MP đột phá và hiệu năng vượt trội cho mọi tác vụ.' },
        3: { id: 3, name: 'Iphone 15', price: 15390000, image: 'img/10.jpg', description: 'Thiết kế bo tròn mềm mại, cổng sạc USB-C tiện lợi và hiệu năng được nâng cấp toàn diện.' },
        4: { id: 4, name: 'Samsung S25 Ultra', price: 12500000, image: 'img/20.jpg', description: 'Vua nhiếp ảnh di động với hệ thống camera zoom quang học ấn tượng, đi kèm bút S Pen đa năng.' },
        5: { id: 5, name: 'Airpods Pro 3', price: 6790000, image: 'img/11.jpg', description: 'Chất âm đỉnh cao, khả năng chống ồn chủ động thông minh và thiết kế vừa vặn hoàn hảo.' },
        6: { id: 6, name: 'AirPods Max USB C', price: 12990000, image: 'img/12.jpg', description: 'Trải nghiệm âm thanh không gian sống động như trong rạp hát với thiết kế sang trọng và cao cấp.' }
        // Bạn có thể thêm các sản phẩm khác vào đây với ID tăng dần
    };

    // ===================================
    // LOGIC GIỎ HÀNG (Sử dụng localStorage)
    // ===================================
    // Lấy giỏ hàng từ localStorage, nếu không có thì tạo mảng rỗng
    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // Lưu giỏ hàng vào localStorage
    function saveCart() {
        localStorage.setItem('cart', JSON.stringify(cart));
    }

    // Cập nhật số lượng trên icon giỏ hàng
    function updateCartIcon() {
        const totalQuantity = cart.reduce((sum, item) => sum + item.quantity, 0);
        const cartCountElements = document.querySelectorAll('.cart-count');
        cartCountElements.forEach(el => {
            el.textContent = totalQuantity;
        });
    }

    // Thêm sản phẩm vào giỏ hàng
    function addToCart(productId, quantity) {
        const productData = allProducts[productId];
        if (!productData) {
            console.error('Sản phẩm không tồn tại!');
            return;
        }

        const existingItem = cart.find(item => item.id === productId);
        if (existingItem) {
            // Nếu sản phẩm đã có, tăng số lượng
            existingItem.quantity += quantity;
        } else {
            // Nếu chưa có, thêm sản phẩm mới vào giỏ
            cart.push({ ...productData, quantity: quantity });
        }
        saveCart();
        updateCartIcon();
        alert('Sản phẩm đã được thêm vào giỏ hàng!');
    }

    // ===================================
    // CHỨC NĂNG TRANG CHỦ (SLIDER)
    // ===================================
    const sliderWrapper = document.querySelector('.slider-wrapper');
    if (sliderWrapper) { // Chỉ chạy nếu có slider trên trang
        const dots = document.querySelectorAll('.dot');
        const slides = document.querySelectorAll('.slide');
        let currentSlide = 0;
        const slideCount = slides.length;
        const slideInterval = 5000;

        function goToSlide(slideIndex) {
            sliderWrapper.style.transform = `translateX(-${slideIndex * (100 / slideCount)}%)`;
            dots.forEach(dot => dot.classList.remove('active'));
            dots[slideIndex].classList.add('active');
            currentSlide = slideIndex;
        }

        function nextSlide() {
            let nextIndex = (currentSlide + 1) % slideCount;
            goToSlide(nextIndex);
        }

        let slideTimer = setInterval(nextSlide, slideInterval);

        dots.forEach(dot => {
            dot.addEventListener('click', () => {
                const slideIndex = parseInt(dot.dataset.slide);
                goToSlide(slideIndex);
                clearInterval(slideTimer);
                slideTimer = setInterval(nextSlide, slideInterval);
            });
        });
    }

    // ===================================
    // CHỨC NĂNG TRANG CHI TIẾT SẢN PHẨM
    // ===================================
    if (document.body.classList.contains('product-detail-page')) {
        const urlParams = new URLSearchParams(window.location.search);
        const productId = parseInt(urlParams.get('id')); // Lấy 'id' từ URL
        const product = allProducts[productId]; // Tìm sản phẩm trong "database"

        if (product) {
            // Cập nhật thông tin sản phẩm lên trang
            document.getElementById('product-image').src = product.image;
            document.getElementById('product-image').alt = product.name;
            document.getElementById('product-name').textContent = product.name;
            document.getElementById('product-price').textContent = `${product.price.toLocaleString('vi-VN')}đ`;
            document.getElementById('product-description').textContent = product.description;
            
            // Gắn sự kiện cho nút "Thêm vào giỏ hàng"
            document.getElementById('add-to-cart-btn').addEventListener('click', () => {
                const quantity = parseInt(document.getElementById('quantity').value);
                if (quantity > 0) {
                    addToCart(product.id, quantity);
                } else {
                    alert('Số lượng phải lớn hơn 0');
                }
            });
        } else {
            // Nếu không tìm thấy sản phẩm, hiển thị thông báo lỗi
            document.querySelector('.product-detail-container').innerHTML = '<h1>Sản phẩm không tồn tại hoặc đã bị xóa.</h1>';
        }
    }

    // ===================================
    // CHỨC NĂNG TRANG GIỎ HÀNG
    // ===================================
    if (document.body.classList.contains('cart-page')) {
        const cartItemsContainer = document.getElementById('cart-items-container');
        const subtotalPriceEl = document.getElementById('subtotal-price');
        const totalPriceEl = document.getElementById('total-price');

        function displayCartItems() {
            cartItemsContainer.innerHTML = ''; // Xóa các item cũ
            if (cart.length === 0) {
                cartItemsContainer.innerHTML = '<p>Giỏ hàng của bạn đang trống.</p>';
                subtotalPriceEl.textContent = '0đ';
                totalPriceEl.textContent = '0đ';
                return;
            }

            let subtotal = 0;
            cart.forEach(item => {
                const itemTotal = item.price * item.quantity;
                subtotal += itemTotal;
                const cartItemHTML = `
                    <div class="cart-item">
                        <img src="${item.image}" alt="${item.name}">
                        <div class="cart-item-details">
                            <p class="product-name">${item.name}</p>
                            <p class="product-price">${item.price.toLocaleString('vi-VN')}đ</p>
                        </div>
                        <div class="quantity-selector">
                            <input type="number" value="${item.quantity}" min="1" data-id="${item.id}" class="cart-quantity-input">
                        </div>
                        <p class="item-total-price">${itemTotal.toLocaleString('vi-VN')}đ</p>
                        <button class="remove-item-btn" data-id="${item.id}">×</button>
                    </div>`;
                cartItemsContainer.insertAdjacentHTML('beforeend', cartItemHTML);
            });
            
            // Cập nhật tổng tiền
            subtotalPriceEl.textContent = `${subtotal.toLocaleString('vi-VN')}đ`;
            totalPriceEl.textContent = `${subtotal.toLocaleString('vi-VN')}đ`;
        }

        // Hàm xử lý các sự kiện trong giỏ hàng (thay đổi số lượng, xóa)
        function handleCartActions(event) {
            // Thay đổi số lượng
            if (event.target.classList.contains('cart-quantity-input')) {
                const productId = parseInt(event.target.dataset.id);
                const newQuantity = parseInt(event.target.value);
                const itemInCart = cart.find(item => item.id === productId);
                if (itemInCart && newQuantity > 0) {
                    itemInCart.quantity = newQuantity;
                    saveCart();
                    displayCartItems(); // Vẽ lại giỏ hàng
                    updateCartIcon();
                }
            }
            // Xóa sản phẩm
            if (event.target.classList.contains('remove-item-btn')) {
                const productId = parseInt(event.target.dataset.id);
                cart = cart.filter(item => item.id !== productId); // Lọc và xóa sản phẩm
                saveCart();
                displayCartItems(); // Vẽ lại giỏ hàng
                updateCartIcon();
            }
        }
        
        // Gắn sự kiện listener cho toàn bộ container
        cartItemsContainer.addEventListener('change', handleCartActions);
        cartItemsContainer.addEventListener('click', handleCartActions);
        
        // Hiển thị các sản phẩm trong giỏ hàng khi tải trang
        displayCartItems();
    }

    // Luôn cập nhật icon giỏ hàng trên mọi trang khi tải xong
    updateCartIcon();
});