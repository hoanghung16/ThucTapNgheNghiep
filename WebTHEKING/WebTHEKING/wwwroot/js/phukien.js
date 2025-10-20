document.addEventListener('DOMContentLoaded', function() {
    // Lấy các phần tử cần thiết từ trang
    const categoryFilter = document.getElementById('category-filter');
    const sortFilter = document.getElementById('sort-filter');
    const productGrid = document.getElementById('product-grid');
    
    // Chuyển danh sách các sản phẩm thành một mảng để dễ xử lý
    const allProducts = Array.from(productGrid.querySelectorAll('.product-card'));

    // Hàm chính để lọc và sắp xếp
    function filterAndSortProducts() {
        const selectedCategory = categoryFilter.value;
        const selectedSort = sortFilter.value;

        // BƯỚC 1: LỌC SẢN PHẨM THEO DANH MỤC
        let filteredProducts = allProducts.filter(product => {
            if (selectedCategory === 'all') {
                return true; // Hiển thị tất cả nếu chọn "Tất cả"
            }
            return product.dataset.category === selectedCategory;
        });

        // BƯỚC 2: SẮP XẾP CÁC SẢN PHẨM ĐÃ LỌC
        filteredProducts.sort((a, b) => {
            const priceA = parseFloat(a.dataset.price);
            const priceB = parseFloat(b.dataset.price);

            if (selectedSort === 'price-asc') {
                return priceA - priceB; // Sắp xếp giá tăng dần
            } else if (selectedSort === 'price-desc') {
                return priceB - priceA; // Sắp xếp giá giảm dần
            }
            return 0; // Giữ nguyên thứ tự nếu là "Mặc định"
        });
        
        // BƯỚC 3: HIỂN THỊ LẠI CÁC SẢN PHẨM LÊN GIAO DIỆN
        // Xóa tất cả sản phẩm hiện có khỏi lưới
        productGrid.innerHTML = '';
        
        // Thêm lại các sản phẩm đã được lọc và sắp xếp vào lưới
        filteredProducts.forEach(product => {
            productGrid.appendChild(product);
        });
    }

    // Gắn sự kiện 'change' cho cả hai bộ lọc
    // Bất cứ khi nào người dùng thay đổi lựa chọn, hàm sẽ được gọi
    categoryFilter.addEventListener('change', filterAndSortProducts);
    sortFilter.addEventListener('change', filterAndSortProducts);
});