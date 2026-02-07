# ĐÁNH GIÁ SCHEMA - HOSPITAL MANAGEMENT SYSTEM

## 📊 TỔNG QUAN: 17 BẢNG

### ✅ ĐÃ CÓ (8 bảng):
1. ✅ **Roles** - Quản lý vai trò
2. ✅ **Users** - Tài khoản đăng nhập
3. ✅ **UserProfiles** - Thông tin cá nhân
4. ✅ **Departments** - Khoa phòng
5. ✅ **Doctors** - Bác sĩ
6. ✅ **Patients** - Bệnh nhân
7. ✅ **DoctorSchedules** - Lịch làm việc
8. ✅ **Appointments** - Lịch hẹn

### ❌ CÒN THIẾU (9 bảng):
9. ❌ **SystemConfigurations** - Cấu hình hệ thống
10. ❌ **MedicalRecords** - Bệnh án điện tử
11. ❌ **Medicines** - Danh mục thuốc
12. ❌ **Prescriptions** - Đơn thuốc
13. ❌ **PrescriptionItems** - Chi tiết đơn thuốc
14. ❌ **Invoices** - Hóa đơn
15. ❌ **InvoiceItems** - Chi tiết hóa đơn
16. ❌ **Payments** - Thanh toán

---

## 🔍 ĐÁNH GIÁ CHI TIẾT

### 1. SCHEMA ĐÁNH GIÁ TỔNG THỂ: ⭐⭐⭐⭐ (4/5)

#### ✅ ĐIỂM MẠNH:
- **Phân tách rõ ràng**: Core System → Hospital Structure → Patient → Scheduling → Clinical → Financial
- **Quan hệ hợp lý**: Foreign keys được thiết kế đúng
- **Đủ cho hệ thống cơ bản**: Có thể triển khai được workflow chính

#### ⚠️ CẦN CẢI THIỆN:
1. **Thiếu bảng Audit/Logging**: Không có bảng ghi lại lịch sử thay đổi
2. **Thiếu bảng Notifications**: Không có hệ thống thông báo
3. **Thiếu bảng LabTests**: Nếu cần xét nghiệm
4. **Thiếu bảng Rooms**: Nếu cần quản lý phòng khám
5. **SystemConfigurations**: Nên có để linh hoạt cấu hình

---

## 🚀 CÁC LOGIC CẦN BỔ SUNG

### PHẦN 1: CLINICAL WORKFLOW (Quy trình khám bệnh)

#### 1.1. Medical Records (Bệnh án điện tử)
**Entities cần tạo:**
- `MedicalRecord` entity
- `MedicalRecordRepository`
- DTOs: `MedicalRecordDto`, `CreateMedicalRecordDto`

**Logic cần implement:**
- ✅ Tạo bệnh án sau khi appointment completed
- ✅ Xem lịch sử bệnh án của bệnh nhân
- ✅ Cập nhật bệnh án (chỉ doctor)
- ✅ Tìm kiếm bệnh án theo triệu chứng/chẩn đoán

**Commands/Queries:**
- `CreateMedicalRecordCommand`
- `GetMedicalRecordsByPatientIdQuery`
- `GetMedicalRecordByIdQuery`
- `UpdateMedicalRecordCommand`

---

#### 1.2. Prescription Management (Quản lý đơn thuốc)
**Entities cần tạo:**
- `Medicine` entity
- `Prescription` entity
- `PrescriptionItem` entity
- Repositories cho cả 3

**Logic cần implement:**
- ✅ Tạo đơn thuốc từ MedicalRecord
- ✅ Thêm/xóa/sửa thuốc trong đơn
- ✅ Kiểm tra tồn kho thuốc
- ✅ Tính tổng tiền đơn thuốc
- ✅ Quản lý tồn kho thuốc (nhập/xuất)

**Commands/Queries:**
- `CreatePrescriptionCommand`
- `AddPrescriptionItemCommand`
- `RemovePrescriptionItemCommand`
- `GetPrescriptionByIdQuery`
- `GetMedicinesQuery` (với filter/search)
- `UpdateMedicineStockCommand`

---

### PHẦN 2: FINANCIAL & BILLING (Tài chính & Thanh toán)

#### 2.1. Invoice Management (Quản lý hóa đơn)
**Entities cần tạo:**
- `Invoice` entity
- `InvoiceItem` entity
- Repositories

**Logic cần implement:**
- ✅ Tạo hóa đơn tự động sau appointment
- ✅ Thêm các mục vào hóa đơn (phí khám, thuốc, xét nghiệm)
- ✅ Tính tổng tiền hóa đơn
- ✅ Áp dụng BHYT (nếu có)
- ✅ Xuất hóa đơn PDF

**Commands/Queries:**
- `CreateInvoiceCommand`
- `AddInvoiceItemCommand`
- `GetInvoiceByIdQuery`
- `GetInvoicesByPatientIdQuery`
- `CalculateInvoiceTotalCommand`

---

#### 2.2. Payment Processing (Xử lý thanh toán)
**Entities cần tạo:**
- `Payment` entity
- Repository

**Logic cần implement:**
- ✅ Thanh toán hóa đơn (một phần hoặc toàn bộ)
- ✅ Tích hợp cổng thanh toán (VNPay, MoMo)
- ✅ Xử lý hoàn tiền
- ✅ Lịch sử thanh toán
- ✅ Báo cáo doanh thu

**Commands/Queries:**
- `CreatePaymentCommand`
- `ProcessPaymentCommand` (với VNPay)
- `GetPaymentsByInvoiceIdQuery`
- `RefundPaymentCommand`
- `GetRevenueReportQuery`

---

### PHẦN 3: SYSTEM CONFIGURATION

#### 3.1. SystemConfigurations
**Entities cần tạo:**
- `SystemConfiguration` entity
- Repository

**Logic cần implement:**
- ✅ Lấy cấu hình hệ thống
- ✅ Cập nhật cấu hình (chỉ Admin)
- ✅ Cache cấu hình để tối ưu

**Commands/Queries:**
- `GetSystemConfigurationQuery`
- `UpdateSystemConfigurationCommand`

---

### PHẦN 4: BỔ SUNG LOGIC CHO CÁC TÍNH NĂNG HIỆN TẠI

#### 4.1. Appointment Enhancements
**Cần bổ sung:**
- ✅ Check-in appointment (bệnh nhân đến)
- ✅ Complete appointment (hoàn thành khám)
- ✅ No-show handling (bệnh nhân không đến)
- ✅ Reschedule appointment (đổi lịch)
- ✅ Reminder notifications (nhắc nhở trước 1 ngày)

**Commands cần thêm:**
- `CheckInAppointmentCommand`
- `CompleteAppointmentCommand`
- `RescheduleAppointmentCommand`
- `SendAppointmentReminderCommand`

---

#### 4.2. Patient Management Enhancements
**Cần bổ sung:**
- ✅ Tự động tạo Patient khi register (nếu role = Patient)
- ✅ Generate PatientCode tự động
- ✅ Quản lý hồ sơ bệnh nhân đầy đủ
- ✅ Tìm kiếm bệnh nhân

**Commands/Queries:**
- `CreatePatientFromUserCommand`
- `GetPatientByCodeQuery`
- `SearchPatientsQuery`

---

#### 4.3. Doctor Management Enhancements
**Cần bổ sung:**
- ✅ Quản lý lịch làm việc định kỳ (recurring schedules)
- ✅ Xem lịch khám của bác sĩ
- ✅ Thống kê số lượng khám

**Commands/Queries:**
- `CreateRecurringScheduleCommand`
- `GetDoctorAppointmentsQuery`
- `GetDoctorStatisticsQuery`

---

## 📋 CHECKLIST TRIỂN KHAI

### Phase 1: Clinical Workflow (Ưu tiên cao)
- [ ] Tạo MedicalRecord entities & repositories
- [ ] Tạo Medicine, Prescription, PrescriptionItem entities
- [ ] Implement CreateMedicalRecord logic
- [ ] Implement Prescription management
- [ ] Implement Medicine inventory

### Phase 2: Financial System (Ưu tiên cao)
- [ ] Tạo Invoice, InvoiceItem entities
- [ ] Tạo Payment entity
- [ ] Implement Invoice generation
- [ ] Implement Payment processing
- [ ] Implement Revenue reporting

### Phase 3: System Configuration (Ưu tiên trung bình)
- [ ] Tạo SystemConfiguration entity
- [ ] Implement configuration management

### Phase 4: Enhancements (Ưu tiên thấp)
- [ ] Appointment check-in/complete
- [ ] Patient auto-creation
- [ ] Recurring schedules
- [ ] Notifications system

---

## 🎯 KẾT LUẬN

**Schema hiện tại: 8/10** - Tốt cho hệ thống cơ bản, nhưng cần bổ sung:
1. ✅ Clinical workflow (MedicalRecords, Prescriptions)
2. ✅ Financial system (Invoices, Payments)
3. ✅ System configuration
4. ✅ Enhancements cho các tính năng hiện có

**Ưu tiên triển khai:**
1. **MedicalRecords** - Core của hệ thống khám bệnh
2. **Prescriptions** - Quản lý đơn thuốc
3. **Invoices & Payments** - Tài chính
4. **SystemConfigurations** - Cấu hình linh hoạt


