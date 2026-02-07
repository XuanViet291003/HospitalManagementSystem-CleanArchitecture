# 🧪 TEST SCENARIOS - HOSPITAL MANAGEMENT SYSTEM

## 📋 SCENARIO 1: BỆNH NHÂN ĐẶT LỊCH VÀ KHÁM BỆNH HOÀN CHỈNH

### Bước 1: Đăng ký tài khoản
```http
POST http://localhost:5000/api/users/register
Content-Type: application/json

{
  "email": "patient.test@example.com",
  "password": "Test123!@#",
  "fullname": "Nguyễn Văn Test",
  "roleName": "Patient"
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "userId": 1 }`
- ✅ Tự động tạo Patient với PatientCode (VD: BN202412030001)

---

### Bước 2: Đăng nhập
```http
POST http://localhost:5000/api/users/login
Content-Type: application/json

{
  "email": "patient.test@example.com",
  "password": "Test123!@#"
}
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Response có `token`
- ✅ Lưu token để dùng cho các request sau

---

### Bước 3: Xem danh sách khoa
```http
GET http://localhost:5000/api/departments
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Trả về danh sách departments

---

### Bước 4: Xem bác sĩ trong khoa
```http
GET http://localhost:5000/api/doctors/department/1
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Trả về danh sách doctors của khoa

---

### Bước 5: Xem lịch trống
```http
GET http://localhost:5000/api/appointments/doctor/1/available-slots?date=2024-12-20&slotDurationMinutes=30
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Trả về danh sách slots với `isAvailable: true/false`

---

### Bước 6: Đặt lịch khám
```http
POST http://localhost:5000/api/appointments
Authorization: Bearer {token}
Content-Type: application/json

{
  "doctorId": 1,
  "appointmentTime": "2024-12-20T08:00:00Z",
  "durationMinutes": 30,
  "notes": "Khám tổng quát"
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "appointmentId": 1 }`
- ✅ Appointment status = 0 (Scheduled)

---

### Bước 7: Xem lịch hẹn của mình
```http
GET http://localhost:5000/api/appointments/patient/1
Authorization: Bearer {token}
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Trả về appointment vừa tạo với status = 0

---

## 📋 SCENARIO 2: QUY TRÌNH KHÁM BỆNH (RECEPTIONIST + DOCTOR)

### Bước 1: Check-in Appointment
```http
POST http://localhost:5000/api/appointments/1/check-in
Authorization: Bearer {admin_token}
```

**Kỳ vọng:**
- ✅ Status: 204 No Content
- ✅ Appointment status = 3 (CheckedIn)

---

### Bước 2: Complete Appointment
```http
POST http://localhost:5000/api/appointments/1/complete
Authorization: Bearer {doctor_token}
```

**Kỳ vọng:**
- ✅ Status: 204 No Content
- ✅ Appointment status = 1 (Completed)

---

### Bước 3: Tạo Medical Record
```http
POST http://localhost:5000/api/medicalrecords
Authorization: Bearer {doctor_token}
Content-Type: application/json

{
  "appointmentId": 1,
  "symptoms": "Đau đầu, sốt nhẹ, ho",
  "diagnosis": "Cảm cúm thông thường",
  "treatmentPlan": "Nghỉ ngơi, uống nhiều nước, dùng thuốc hạ sốt",
  "followUpDate": "2024-12-27T00:00:00Z"
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "medicalRecordId": 1 }`

---

### Bước 4: Tạo Prescription
```http
POST http://localhost:5000/api/prescriptions
Authorization: Bearer {doctor_token}
Content-Type: application/json

{
  "medicalRecordId": 1,
  "items": [
    {
      "medicineId": 1,
      "quantity": 20,
      "dosage": "Sáng 1 viên, tối 1 viên sau ăn",
      "notes": "Uống sau khi ăn no"
    }
  ]
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "prescriptionId": 1 }`
- ✅ Tồn kho thuốc giảm (Stock - Quantity)

---

## 📋 SCENARIO 3: QUY TRÌNH THANH TOÁN

### Bước 1: Tạo Invoice
```http
POST http://localhost:5000/api/invoices
Authorization: Bearer {receptionist_token}
Content-Type: application/json

{
  "appointmentId": 1,
  "items": [
    {
      "itemDescription": "Thuốc Paracetamol",
      "itemType": "Medicine",
      "quantity": 1,
      "unitPrice": 100000
    }
  ],
  "dueDate": "2024-12-25T00:00:00Z"
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "invoiceId": 1 }`
- ✅ Tự động thêm phí khám (Consultation Fee)
- ✅ InvoiceCode tự động (VD: INV202412200001)
- ✅ TotalAmount = ConsultationFee + MedicinePrice

---

### Bước 2: Xem Invoice
```http
GET http://localhost:5000/api/invoices/1
Authorization: Bearer {token}
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Có đầy đủ items (Consultation + Medicine)
- ✅ TotalAmount đúng

---

### Bước 3: Thanh toán
```http
POST http://localhost:5000/api/payments
Authorization: Bearer {receptionist_token}
Content-Type: application/json

{
  "invoiceId": 1,
  "amount": 300000,
  "paymentMethod": "Cash",
  "transactionCode": null
}
```

**Kỳ vọng:**
- ✅ Status: 201 Created
- ✅ Response: `{ "paymentId": 1 }`
- ✅ Invoice status = 1 (Paid) nếu thanh toán đủ

---

### Bước 4: Xem lịch sử thanh toán
```http
GET http://localhost:5000/api/payments/invoice/1
Authorization: Bearer {token}
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Trả về payment vừa tạo

---

## 📋 SCENARIO 4: BÁO CÁO DOANH THU (ADMIN)

### Xem Revenue Report
```http
GET http://localhost:5000/api/payments/revenue?startDate=2024-12-01&endDate=2024-12-31
Authorization: Bearer {admin_token}
```

**Kỳ vọng:**
- ✅ Status: 200 OK
- ✅ Có totalRevenue, totalPayments
- ✅ Có breakdown theo payment method
- ✅ Có dailyRevenues

---

## 📋 SCENARIO 5: TEST VALIDATION & ERROR HANDLING

### Test 1: Đặt lịch trùng
```http
POST /api/appointments
Body: { doctorId: 1, appointmentTime: "2024-12-20T08:00:00Z" }
→ Đặt lần 1: ✅ Success
→ Đặt lần 2 (cùng thời gian): ❌ Conflict error
```

### Test 2: Tạo prescription thiếu tồn kho
```http
POST /api/prescriptions
Body: { medicalRecordId: 1, items: [{ medicineId: 1, quantity: 10000 }] }
→ ❌ Error: "Thuốc 'Paracetamol' không đủ tồn kho. Tồn kho hiện tại: 980, yêu cầu: 10000"
```

### Test 3: Thanh toán vượt quá số tiền
```http
POST /api/payments
Body: { invoiceId: 1, amount: 1000000 }
→ ❌ Error: "Số tiền thanh toán vượt quá số tiền còn lại"
```

### Test 4: Tạo medical record khi appointment chưa complete
```http
POST /api/medicalrecords
Body: { appointmentId: 1 } (appointment status = Scheduled)
→ ❌ Error: "Chỉ có thể tạo bệnh án cho lịch hẹn đã hoàn thành"
```

---

## 📋 SCENARIO 6: TEST AUTO-GENERATION

### Test PatientCode Generation
```
1. Register Patient 1 → PatientCode: BN202412030001
2. Register Patient 2 → PatientCode: BN202412030002
3. Register Patient 3 (ngày khác) → PatientCode: BN202412040001
```

### Test InvoiceCode Generation
```
1. Create Invoice 1 → InvoiceCode: INV202412200001
2. Create Invoice 2 → InvoiceCode: INV202412200002
3. Create Invoice 3 (ngày khác) → InvoiceCode: INV202412210001
```

---

## 📋 SCENARIO 7: TEST WORKFLOW HOÀN CHỈNH

### Full Workflow Test
```
1. ✅ Register Patient
2. ✅ Login
3. ✅ Get Departments
4. ✅ Get Doctors
5. ✅ Get Available Slots
6. ✅ Create Appointment
7. ✅ Check-in Appointment
8. ✅ Complete Appointment
9. ✅ Create Medical Record
10. ✅ Create Prescription
11. ✅ Create Invoice
12. ✅ Create Payment
13. ✅ Get Revenue Report
```

**Kỳ vọng:**
- ✅ Tất cả bước đều thành công
- ✅ Data consistency giữa các bảng
- ✅ Status transitions đúng
- ✅ Auto-calculations đúng

---

## 🎯 TESTING CHECKLIST

### Authentication & Authorization
- [ ] Register thành công
- [ ] Login thành công, có token
- [ ] Token hợp lệ cho các protected endpoints
- [ ] Token không hợp lệ → 401 Unauthorized
- [ ] Role không đủ quyền → 403 Forbidden

### Appointment Flow
- [ ] Đặt lịch thành công
- [ ] Không thể đặt lịch trùng
- [ ] Check-in chỉ khi status = Scheduled
- [ ] Complete chỉ khi status = CheckedIn
- [ ] Hủy lịch thành công

### Medical Records
- [ ] Tạo medical record chỉ khi appointment = Completed
- [ ] Không thể tạo 2 medical records cho 1 appointment
- [ ] Xem medical records của patient

### Prescriptions
- [ ] Tạo prescription thành công
- [ ] Validation tồn kho đúng
- [ ] Tồn kho tự động trừ
- [ ] Tính tổng tiền đúng

### Invoices
- [ ] Tạo invoice tự động thêm consultation fee
- [ ] Auto-generate invoice code
- [ ] Thêm items vào invoice
- [ ] Tính total amount đúng

### Payments
- [ ] Thanh toán một phần thành công
- [ ] Thanh toán đủ → Invoice status = Paid
- [ ] Không thể thanh toán vượt quá
- [ ] Revenue report chính xác

---

## 🐛 COMMON ERRORS & SOLUTIONS

### Error: "Email đã được đăng ký"
**Solution:** Dùng email khác hoặc xóa user cũ

### Error: "Không tìm thấy bệnh nhân với UserId"
**Solution:** Đảm bảo đã register với role = Patient (auto-create Patient)

### Error: "Thời gian đặt lịch này đã được đặt"
**Solution:** Chọn slot khác hoặc doctor khác

### Error: "Thuốc không đủ tồn kho"
**Solution:** Giảm quantity hoặc nhập thêm thuốc

### Error: "Hóa đơn đã được thanh toán đầy đủ"
**Solution:** Kiểm tra invoice status trước khi thanh toán

---

## 📊 EXPECTED DATABASE STATE

Sau khi chạy full workflow, database sẽ có:
- ✅ 1 User (Patient)
- ✅ 1 Patient (auto-created)
- ✅ 1 Appointment (status = Completed)
- ✅ 1 MedicalRecord
- ✅ 1 Prescription với items
- ✅ Medicines với stock đã trừ
- ✅ 1 Invoice (status = Paid)
- ✅ 1 Payment

---

## 🎉 KẾT LUẬN

Tất cả các scenarios đã được thiết kế để test đầy đủ workflow của hệ thống. Sử dụng các test cases này để đảm bảo hệ thống hoạt động đúng như mong đợi.


