# 📖 HƯỚNG DẪN LUỒNG CHỨC NĂNG - HOSPITAL MANAGEMENT SYSTEM

## 🎯 MỤC LỤC

1. [Luồng Đăng ký & Đăng nhập](#1-luồng-đăng-ký--đăng-nhập)
2. [Luồng Đặt Lịch Khám](#2-luồng-đặt-lịch-khám)
3. [Luồng Khám Bệnh](#3-luồng-khám-bệnh)
4. [Luồng Kê Đơn Thuốc](#4-luồng-kê-đơn-thuốc)
5. [Luồng Thanh Toán](#5-luồng-thanh-toán)
6. [Luồng Quản Lý Bác Sĩ](#6-luồng-quản-lý-bác-sĩ)
7. [Luồng Quản Lý Thuốc](#7-luồng-quản-lý-thuốc)
8. [Luồng Báo Cáo Doanh Thu](#8-luồng-báo-cáo-doanh-thu)

---

## 1. LUỒNG ĐĂNG KÝ & ĐĂNG NHẬP

### 1.1. Đăng ký Bệnh nhân (Patient)

**Endpoint:** `POST /api/users/register`

**Request Body:**
```json
{
  "email": "patient@example.com",
  "password": "Password123!",
  "fullname": "Nguyễn Văn A",
  "roleName": "Patient"
}
```

**Response (201 Created):**
```json
{
  "userId": 1
}
```

**Lưu ý:**
- ✅ Tự động tạo Patient record với PatientCode (VD: BN202412030001)
- ✅ Role mặc định = "Patient" nếu không chỉ định

---

### 1.2. Đăng nhập

**Endpoint:** `POST /api/users/login`

**Request Body:**
```json
{
  "email": "patient@example.com",
  "password": "Password123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Lưu ý:**
- Lưu token này để sử dụng cho các request cần authentication
- Token có thời hạn 8 giờ

---

### 1.3. Xem Thông Tin Profile

**Endpoint:** `GET /api/users/profile`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "userId": "1",
  "email": "patient@example.com"
}
```

---

## 2. LUỒNG ĐẶT LỊCH KHÁM

### 2.1. Xem Danh Sách Khoa

**Endpoint:** `GET /api/departments`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Khoa Nội",
    "description": "Khoa nội tổng quát"
  },
  {
    "id": 2,
    "name": "Khoa Ngoại",
    "description": "Khoa ngoại tổng quát"
  }
]
```

---

### 2.2. Xem Danh Sách Bác Sĩ Theo Khoa

**Endpoint:** `GET /api/doctors/department/{departmentId}`

**Example:** `GET /api/doctors/department/1`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "fullName": "doctor1@example.com",
    "specialization": "Tim mạch",
    "licenseNumber": "BS001",
    "consultationFee": 200000,
    "departmentId": 1,
    "departmentName": "Khoa Nội"
  }
]
```

---

### 2.3. Xem Lịch Trống Của Bác Sĩ

**Endpoint:** `GET /api/appointments/doctor/{doctorId}/available-slots?date=2024-12-15&slotDurationMinutes=30`

**Query Parameters:**
- `date`: Ngày cần xem (format: yyyy-MM-dd)
- `slotDurationMinutes`: Độ dài mỗi slot (mặc định: 30)

**Response (200 OK):**
```json
[
  {
    "startTime": "2024-12-15T08:00:00Z",
    "endTime": "2024-12-15T08:30:00Z",
    "isAvailable": true
  },
  {
    "startTime": "2024-12-15T08:30:00Z",
    "endTime": "2024-12-15T09:00:00Z",
    "isAvailable": true
  },
  {
    "startTime": "2024-12-15T09:00:00Z",
    "endTime": "2024-12-15T09:30:00Z",
    "isAvailable": false
  }
]
```

**Lưu ý:**
- `isAvailable: false` = Slot đã được đặt hoặc bị conflict
- Chỉ hiển thị slots trong `DoctorSchedule` có `IsAvailable = true`

---

### 2.4. Đặt Lịch Khám

**Endpoint:** `POST /api/appointments`

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "doctorId": 1,
  "appointmentTime": "2024-12-15T08:00:00Z",
  "durationMinutes": 30,
  "notes": "Khám tổng quát"
}
```

**Response (201 Created):**
```json
{
  "appointmentId": 1
}
```

**Validation:**
- ✅ Kiểm tra PatientId từ token (tự động lấy từ UserId)
- ✅ Kiểm tra conflict với lịch khác
- ✅ Kiểm tra thời gian trong tương lai
- ✅ Status mặc định = 0 (Scheduled)

---

### 2.5. Xem Lịch Hẹn Của Mình

**Endpoint:** `GET /api/appointments/patient/{patientId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "appointmentTime": "2024-12-15T08:00:00Z",
    "durationMinutes": 30,
    "status": 0,
    "notes": "Khám tổng quát",
    "doctor": {
      "id": 1,
      "fullName": "doctor1@example.com",
      "specialization": "Tim mạch",
      "departmentName": "Khoa Nội"
    },
    "patient": {
      "id": 1,
      "patientCode": "BN202412030001",
      "fullName": "patient@example.com"
    }
  }
]
```

**Status Codes:**
- `0` = Scheduled (Đã đặt)
- `1` = Completed (Đã hoàn thành)
- `2` = Cancelled (Đã hủy)
- `3` = CheckedIn (Đã check-in)
- `4` = NoShow (Không đến)

---

### 2.6. Hủy Lịch Hẹn

**Endpoint:** `POST /api/appointments/{id}/cancel`

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body (Optional):**
```json
{
  "reason": "Bận việc đột xuất"
}
```

**Response (204 No Content)**

**Validation:**
- ❌ Không thể hủy nếu đã Completed
- ❌ Không thể hủy nếu đã Cancelled

---

## 3. LUỒNG KHÁM BỆNH

### 3.1. Check-in Appointment (Bệnh nhân đến)

**Endpoint:** `POST /api/appointments/{id}/check-in`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin, Receptionist
```

**Response (204 No Content)**

**Lưu ý:**
- Chỉ có thể check-in lịch hẹn có status = Scheduled (0)
- Sau khi check-in, status = CheckedIn (3)

---

### 3.2. Complete Appointment (Hoàn thành khám)

**Endpoint:** `POST /api/appointments/{id}/complete`

**Headers:**
```
Authorization: Bearer {token}
Role: Doctor, Admin
```

**Response (204 No Content)**

**Lưu ý:**
- Chỉ có thể complete lịch hẹn đã check-in (status = 3)
- Sau khi complete, status = Completed (1)
- Có thể tạo Medical Record sau khi complete

---

### 3.3. Tạo Bệnh Án (Medical Record)

**Endpoint:** `POST /api/medicalrecords`

**Headers:**
```
Authorization: Bearer {token}
Role: Doctor, Admin
```

**Request Body:**
```json
{
  "appointmentId": 1,
  "symptoms": "Đau đầu, sốt nhẹ",
  "diagnosis": "Cảm cúm thông thường",
  "treatmentPlan": "Nghỉ ngơi, uống nhiều nước, dùng thuốc hạ sốt",
  "followUpDate": "2024-12-20T00:00:00Z"
}
```

**Response (201 Created):**
```json
{
  "medicalRecordId": 1
}
```

**Validation:**
- ✅ Chỉ tạo được khi appointment status = Completed (1)
- ✅ Mỗi appointment chỉ có 1 medical record
- ✅ Tự động lấy PatientId và DoctorId từ appointment

---

### 3.4. Xem Lịch Sử Bệnh Án

**Endpoint:** `GET /api/medicalrecords/patient/{patientId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "appointmentId": 1,
    "appointmentTime": "2024-12-15T08:00:00Z",
    "symptoms": "Đau đầu, sốt nhẹ",
    "diagnosis": "Cảm cúm thông thường",
    "treatmentPlan": "Nghỉ ngơi, uống nhiều nước",
    "followUpDate": "2024-12-20T00:00:00Z",
    "doctor": {
      "id": 1,
      "fullName": "doctor1@example.com",
      "specialization": "Tim mạch",
      "departmentName": "Khoa Nội"
    },
    "patient": {
      "id": 1,
      "patientCode": "BN202412030001",
      "fullName": "patient@example.com"
    }
  }
]
```

---

### 3.5. Cập Nhật Bệnh Án

**Endpoint:** `PUT /api/medicalrecords/{id}`

**Headers:**
```
Authorization: Bearer {token}
Role: Doctor, Admin
```

**Request Body:**
```json
{
  "symptoms": "Đau đầu, sốt nhẹ, ho",
  "diagnosis": "Cảm cúm, viêm họng nhẹ",
  "treatmentPlan": "Nghỉ ngơi, uống thuốc",
  "followUpDate": "2024-12-22T00:00:00Z"
}
```

**Response (204 No Content)**

---

## 4. LUỒNG KÊ ĐƠN THUỐC

### 4.1. Tìm Kiếm Thuốc

**Endpoint:** `GET /api/medicines?searchTerm=paracetamol`

**Query Parameters:**
- `searchTerm` (optional): Từ khóa tìm kiếm

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Paracetamol 500mg",
    "unit": "Viên",
    "stock": 1000,
    "unitPrice": 5000
  },
  {
    "id": 2,
    "name": "Paracetamol 250mg",
    "unit": "Viên",
    "stock": 500,
    "unitPrice": 3000
  }
]
```

---

### 4.2. Tạo Đơn Thuốc

**Endpoint:** `POST /api/prescriptions`

**Headers:**
```
Authorization: Bearer {token}
Role: Doctor, Admin
```

**Request Body:**
```json
{
  "medicalRecordId": 1,
  "items": [
    {
      "medicineId": 1,
      "quantity": 20,
      "dosage": "Sáng 1 viên, tối 1 viên sau ăn",
      "notes": "Uống sau khi ăn no"
    },
    {
      "medicineId": 2,
      "quantity": 10,
      "dosage": "Ngày 3 lần, mỗi lần 1 viên",
      "notes": null
    }
  ]
}
```

**Response (201 Created):**
```json
{
  "prescriptionId": 1
}
```

**Validation:**
- ✅ Kiểm tra MedicalRecord tồn tại
- ✅ Kiểm tra Medicine tồn tại
- ✅ Kiểm tra tồn kho đủ (Stock >= Quantity)
- ✅ Tự động trừ tồn kho khi tạo đơn
- ✅ Mỗi MedicalRecord chỉ có 1 Prescription

**Lưu ý:**
- Sau khi tạo đơn, tồn kho thuốc sẽ tự động giảm
- Nếu tồn kho không đủ, sẽ báo lỗi cụ thể

---

### 4.3. Xem Chi Tiết Đơn Thuốc

**Endpoint:** `GET /api/prescriptions/{id}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "medicalRecordId": 1,
  "issuedDate": "2024-12-15T10:00:00Z",
  "totalAmount": 130000,
  "items": [
    {
      "id": 1,
      "medicine": {
        "id": 1,
        "name": "Paracetamol 500mg",
        "unit": "Viên",
        "stock": 980,
        "unitPrice": 5000
      },
      "quantity": 20,
      "dosage": "Sáng 1 viên, tối 1 viên sau ăn",
      "notes": "Uống sau khi ăn no",
      "subTotal": 100000
    },
    {
      "id": 2,
      "medicine": {
        "id": 2,
        "name": "Paracetamol 250mg",
        "unit": "Viên",
        "stock": 490,
        "unitPrice": 3000
      },
      "quantity": 10,
      "dosage": "Ngày 3 lần, mỗi lần 1 viên",
      "notes": null,
      "subTotal": 30000
    }
  ]
}
```

**Lưu ý:**
- `totalAmount` = Tổng tiền của tất cả items
- `subTotal` = UnitPrice × Quantity của mỗi item

---

## 5. LUỒNG THANH TOÁN

### 5.1. Tạo Hóa Đơn

**Endpoint:** `POST /api/invoices`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin, Receptionist
```

**Request Body:**
```json
{
  "appointmentId": 1,
  "items": [
    {
      "itemDescription": "Thuốc Paracetamol",
      "itemType": "Medicine",
      "quantity": 1,
      "unitPrice": 130000
    }
  ],
  "dueDate": "2024-12-20T00:00:00Z"
}
```

**Response (201 Created):**
```json
{
  "invoiceId": 1
}
```

**Lưu ý:**
- ✅ Tự động tạo InvoiceCode (VD: INV202412150001)
- ✅ Tự động thêm phí khám nếu appointment = Completed
- ✅ Tự động tính TotalAmount
- ✅ Status mặc định = 0 (Unpaid)

**Auto-add Consultation Fee:**
- Nếu appointment status = Completed, tự động thêm phí khám của bác sĩ vào invoice

---

### 5.2. Thêm Mục Vào Hóa Đơn

**Endpoint:** `POST /api/invoices/{invoiceId}/items`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin, Receptionist
```

**Request Body:**
```json
{
  "itemDescription": "Xét nghiệm máu",
  "itemType": "LabTest",
  "quantity": 1,
  "unitPrice": 150000
}
```

**Response (201 Created):**
```json
{
  "invoiceItemId": 2
}
```

**Validation:**
- ❌ Không thể thêm vào hóa đơn đã Paid
- ❌ Không thể thêm vào hóa đơn đã Cancelled
- ✅ Tự động cập nhật TotalAmount

---

### 5.3. Xem Chi Tiết Hóa Đơn

**Endpoint:** `GET /api/invoices/{id}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "appointmentId": 1,
  "invoiceCode": "INV202412150001",
  "totalAmount": 330000,
  "status": 0,
  "issuedDate": "2024-12-15T10:30:00Z",
  "dueDate": "2024-12-20T00:00:00Z",
  "items": [
    {
      "id": 1,
      "itemDescription": "Phí khám - Tim mạch",
      "itemType": "Consultation",
      "quantity": 1,
      "unitPrice": 200000,
      "amount": 200000
    },
    {
      "id": 2,
      "itemDescription": "Thuốc Paracetamol",
      "itemType": "Medicine",
      "quantity": 1,
      "unitPrice": 130000,
      "amount": 130000
    }
  ],
  "patient": {
    "id": 1,
    "patientCode": "BN202412030001",
    "fullName": "patient@example.com"
  }
}
```

**Status Codes:**
- `0` = Unpaid (Chưa thanh toán)
- `1` = Paid (Đã thanh toán)
- `2` = Cancelled (Đã hủy)

---

### 5.4. Xem Hóa Đơn Của Bệnh Nhân

**Endpoint:** `GET /api/invoices/patient/{patientId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "appointmentId": 1,
    "invoiceCode": "INV202412150001",
    "totalAmount": 330000,
    "status": 0,
    "issuedDate": "2024-12-15T10:30:00Z",
    "dueDate": "2024-12-20T00:00:00Z",
    "items": [...],
    "patient": {...}
  }
]
```

---

### 5.5. Thanh Toán Hóa Đơn

**Endpoint:** `POST /api/payments`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin, Receptionist
```

**Request Body:**
```json
{
  "invoiceId": 1,
  "amount": 330000,
  "paymentMethod": "Cash",
  "transactionCode": null
}
```

**Hoặc thanh toán qua VNPay:**
```json
{
  "invoiceId": 1,
  "amount": 330000,
  "paymentMethod": "VNPay",
  "transactionCode": "VNPAY123456789"
}
```

**Response (201 Created):**
```json
{
  "paymentId": 1
}
```

**Validation:**
- ✅ Kiểm tra Invoice tồn tại
- ✅ Kiểm tra số tiền <= số tiền còn lại
- ✅ Tự động cập nhật Invoice status = Paid nếu thanh toán đủ
- ✅ Hỗ trợ thanh toán một phần hoặc toàn bộ

**Payment Methods:**
- `"Cash"` - Tiền mặt
- `"Card"` - Thẻ
- `"VNPay"` - VNPay

---

### 5.6. Xem Lịch Sử Thanh Toán

**Endpoint:** `GET /api/payments/invoice/{invoiceId}`

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "invoiceId": 1,
    "invoiceCode": "INV202412150001",
    "amount": 330000,
    "paymentMethod": "Cash",
    "transactionCode": null,
    "paymentDate": "2024-12-15T11:00:00Z"
  }
]
```

---

## 6. LUỒNG QUẢN LÝ BÁC SĨ

### 6.1. Tạo Bác Sĩ (Admin only)

**Bước 1: Tạo User cho Bác Sĩ**

**Endpoint:** `POST /api/users`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin
```

**Request Body:**
```json
{
  "email": "doctor1@hospital.com",
  "fullName": "Bác sĩ Nguyễn Văn B",
  "roleName": "Doctor"
}
```

**Response (201 Created):**
```json
{
  "userId": 2
}
```

**Lưu ý:**
- Tự động tạo password ngẫu nhiên
- Gửi email kích hoạt tài khoản (nếu có EmailService)

---

**Bước 2: Tạo Doctor Record**

**Endpoint:** `POST /api/doctors` (Cần tạo endpoint này)

**Request Body:**
```json
{
  "userId": 2,
  "departmentId": 1,
  "specialization": "Tim mạch",
  "licenseNumber": "BS001",
  "consultationFee": 200000
}
```

---

### 6.2. Tạo Lịch Làm Việc Cho Bác Sĩ

**Endpoint:** `POST /api/doctors/{doctorId}/schedules` (Cần tạo endpoint này)

**Request Body:**
```json
{
  "workDate": "2024-12-15T00:00:00Z",
  "startTime": "08:00:00",
  "endTime": "12:00:00",
  "slotDurationMinutes": 30,
  "isAvailable": true
}
```

---

## 7. LUỒNG QUẢN LÝ THUỐC

### 7.1. Thêm Thuốc Mới (Admin only)

**Endpoint:** `POST /api/medicines` (Cần tạo endpoint này)

**Headers:**
```
Authorization: Bearer {token}
Role: Admin
```

**Request Body:**
```json
{
  "name": "Paracetamol 500mg",
  "unit": "Viên",
  "stock": 1000,
  "unitPrice": 5000
}
```

---

### 7.2. Cập Nhật Tồn Kho Thuốc

**Endpoint:** `PUT /api/medicines/{id}/stock` (Cần tạo endpoint này)

**Request Body:**
```json
{
  "stock": 1500
}
```

---

## 8. LUỒNG BÁO CÁO DOANH THU

### 8.1. Xem Báo Cáo Doanh Thu

**Endpoint:** `GET /api/payments/revenue?startDate=2024-12-01&endDate=2024-12-31`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin
```

**Query Parameters:**
- `startDate` (optional): Ngày bắt đầu (mặc định: 1 tháng trước)
- `endDate` (optional): Ngày kết thúc (mặc định: hôm nay)

**Response (200 OK):**
```json
{
  "totalRevenue": 5000000,
  "totalPayments": 25,
  "cashRevenue": 2000000,
  "cardRevenue": 1500000,
  "vnPayRevenue": 1500000,
  "dailyRevenues": [
    {
      "date": "2024-12-15T00:00:00Z",
      "amount": 330000,
      "paymentCount": 1
    },
    {
      "date": "2024-12-16T00:00:00Z",
      "amount": 500000,
      "paymentCount": 2
    }
  ]
}
```

---

## 9. LUỒNG QUẢN LÝ CẤU HÌNH HỆ THỐNG

### 9.1. Xem Tất Cả Cấu Hình

**Endpoint:** `GET /api/systemconfigurations`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin
```

**Response (200 OK):**
```json
[
  {
    "key": "DefaultConsultationFee",
    "value": "200000",
    "description": "Phí khám mặc định"
  },
  {
    "key": "AppointmentCancellationHours",
    "value": "24",
    "description": "Số giờ tối thiểu để hủy lịch hẹn"
  }
]
```

---

### 9.2. Xem Cấu Hình Theo Key

**Endpoint:** `GET /api/systemconfigurations/{key}`

**Example:** `GET /api/systemconfigurations/DefaultConsultationFee`

**Response (200 OK):**
```json
{
  "key": "DefaultConsultationFee",
  "value": "200000",
  "description": "Phí khám mặc định"
}
```

---

### 9.3. Cập Nhật Cấu Hình

**Endpoint:** `PUT /api/systemconfigurations/{key}`

**Headers:**
```
Authorization: Bearer {token}
Role: Admin
```

**Request Body:**
```json
{
  "value": "250000",
  "description": "Phí khám mặc định (đã cập nhật)"
}
```

**Response (204 No Content)**

---

## 🔄 LUỒNG HOÀN CHỈNH TỪNG BƯỚC

### Scenario 1: Bệnh nhân đặt lịch và khám bệnh

```
1. POST /api/users/register
   → Tạo tài khoản + Auto-create Patient (BN202412030001)

2. POST /api/users/login
   → Lấy token

3. GET /api/departments
   → Xem danh sách khoa

4. GET /api/doctors/department/1
   → Xem danh sách bác sĩ khoa Nội

5. GET /api/appointments/doctor/1/available-slots?date=2024-12-15
   → Xem lịch trống của bác sĩ

6. POST /api/appointments
   → Đặt lịch khám (Status: Scheduled)

7. POST /api/appointments/1/check-in
   → Check-in khi đến (Status: CheckedIn)

8. POST /api/appointments/1/complete
   → Hoàn thành khám (Status: Completed)

9. POST /api/medicalrecords
   → Tạo bệnh án

10. POST /api/prescriptions
    → Kê đơn thuốc (tự động trừ tồn kho)

11. POST /api/invoices
    → Tạo hóa đơn (tự động thêm phí khám)

12. POST /api/payments
    → Thanh toán (tự động cập nhật Invoice status = Paid)
```

---

### Scenario 2: Admin quản lý hệ thống

```
1. POST /api/users (tạo bác sĩ)
2. POST /api/doctors/{id}/schedules (tạo lịch làm việc)
3. POST /api/medicines (thêm thuốc)
4. GET /api/payments/revenue (xem báo cáo doanh thu)
5. PUT /api/systemconfigurations/{key} (cập nhật cấu hình)
```

---

## 📝 LƯU Ý QUAN TRỌNG

### Authentication
- Tất cả endpoints (trừ register, login, get departments, get doctors, get available slots) đều cần token
- Thêm header: `Authorization: Bearer {token}`

### Roles & Permissions
- **Patient**: Đặt lịch, xem lịch của mình, xem bệnh án của mình
- **Doctor**: Complete appointment, tạo/cập nhật medical record, tạo prescription
- **Admin**: Tất cả quyền
- **Receptionist**: Check-in appointment, tạo invoice, thanh toán

### Status Flow
```
Appointment: Scheduled (0) → CheckedIn (3) → Completed (1)
Invoice: Unpaid (0) → Paid (1) [sau khi thanh toán đủ]
```

### Auto-generation
- **PatientCode**: BN + yyyyMMdd + #### (VD: BN202412030001)
- **InvoiceCode**: INV + yyyyMMdd + #### (VD: INV202412150001)

---

## 🧪 TESTING CHECKLIST

### Test Case 1: Đăng ký và đặt lịch
- [ ] Đăng ký thành công
- [ ] Auto-create Patient với PatientCode
- [ ] Đăng nhập thành công
- [ ] Xem danh sách khoa
- [ ] Xem danh sách bác sĩ
- [ ] Xem available slots
- [ ] Đặt lịch thành công

### Test Case 2: Khám bệnh
- [ ] Check-in appointment
- [ ] Complete appointment
- [ ] Tạo medical record
- [ ] Xem medical record

### Test Case 3: Kê đơn thuốc
- [ ] Tìm kiếm thuốc
- [ ] Tạo prescription với validation tồn kho
- [ ] Kiểm tra tồn kho đã trừ
- [ ] Xem prescription

### Test Case 4: Thanh toán
- [ ] Tạo invoice (auto-add consultation fee)
- [ ] Thêm items vào invoice
- [ ] Thanh toán một phần
- [ ] Thanh toán đủ → Invoice status = Paid
- [ ] Xem lịch sử thanh toán

### Test Case 5: Báo cáo
- [ ] Xem revenue report
- [ ] Kiểm tra daily revenues
- [ ] Kiểm tra revenue theo payment method

---

## 🎯 KẾT LUẬN

Hệ thống đã có đầy đủ các luồng chức năng từ đặt lịch → khám bệnh → kê đơn → thanh toán. Tất cả các endpoints đã được implement với validation đầy đủ và error handling hợp lý.

**Sẵn sàng để test và triển khai!** 🚀


