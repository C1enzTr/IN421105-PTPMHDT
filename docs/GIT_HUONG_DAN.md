# Nhiệm vụ 5 — Git: nhánh, ≥5 commit, rebase, Pull Request

**Sinh viên:** Trần Minh Trung Kiên — **Mã học phần:** IN421105

Chạy trong thư mục gốc của repo bài tập (nơi có starter), sau khi đã chép code vào.

## 0. Tạo nhánh
```bash
git fetch origin
git switch -c feature/solid-refactor origin/main
```

## 1–6. Commit theo Conventional Commits (6 commit, mỗi commit một thay đổi có ý nghĩa)
```bash
# 1
git add docs/PHAN_TICH_SOLID.md
git commit -m "docs(solid): analyze SOLID violations in ClubManager"

# 2
git add EduHub.sln .gitignore EduHub.Ch02/EduHub.Ch02.csproj EduHub.Ch02.Tests/EduHub.Ch02.Tests.csproj
git commit -m "chore: add solution and project files"

# 3
git add EduHub.Ch02/Domain
git commit -m "refactor(domain): move validation rules into Club, Member, Event"

# 4
git add EduHub.Ch02/Abstractions EduHub.Ch02/Infrastructure EduHub.Ch02/Services
git commit -m "refactor(services): split ClubManager into services behind interfaces"

# 5
git add EduHub.Ch02/Patterns EduHub.Ch02/Program.cs
git commit -m "feat(patterns): add Factory, Decorator, Strategy and Observer"

# 6
git add EduHub.Ch02.Tests docs/PR_DESCRIPTION.md docs/GIT_HUONG_DAN.md README.md
git commit -m "test(domain): add xUnit tests using Arrange-Act-Assert"
```
> Nếu starter có `ClubManager.cs` cũ: xoá nó ở commit 4 bằng `git rm <đường dẫn tới ClubManager.cs>`.
> Các commit 3–5 riêng lẻ chưa build được; chỉ cần toàn bộ nhánh build và test xanh.

## Kiểm tra xanh trước khi rebase
```bash
dotnet build
dotnet test
dotnet run --project EduHub.Ch02
```

## Rebase lên main mới nhất
```bash
git fetch origin
git rebase origin/main
# Nếu conflict: mở file -> chọn nội dung đúng -> xoá <<<<<<< ======= >>>>>>>
git add <file>
git rebase --continue          # hoặc: git rebase --abort
dotnet build && dotnet test    # chạy lại sau khi giải quyết conflict
git push -u origin feature/solid-refactor --force-with-lease
```
Nếu có conflict, thêm một dòng vào mô tả PR: "Conflict ở `<file>`: giữ bản ... vì ...; đã chạy lại build + test."

## Mở Pull Request
- Tiêu đề: `refactor: split ClubManager into services`
- Mô tả: dán nội dung `docs/PR_DESCRIPTION.md` (đủ What / Why / How to test)

## ≥ 2 nhận xét review trên PR của chính mình
Vào tab **Files changed**, bấm dấu `+` cạnh dòng code, chọn *Start a review*. Hai nhận xét gợi ý (viết lại bằng lời của bạn):

1. Trên `Patterns/NotificationServiceFactory.cs` (khối `switch`):
   > Chỉ còn một `switch` duy nhất ở đây nên nơi gọi đạt OCP. Nếu số kênh tăng nhiều thì có thể đăng ký kênh
   > bằng DI container (Chương 3) để khỏi sửa factory. Hiện tại có 3 kênh nên giữ cách này là hợp lý.
2. Trên `Services/ClubService.cs` (constructor):
   > `ClubService` nhận 4 interface qua constructor nên unit test dùng được bản giả (xem `ServiceTests`).
   > Nếu số dependency tăng quá 5 thì cần xem lại trách nhiệm của class (dấu hiệu vi phạm SRP).

Sau đó chọn **Comment**, tự trả lời từng nhận xét, rồi gửi link PR lên LMS.
