<template>
  <div style="width:100%">
    <snail-simple-list-crud search-api="getAllUserInfo"
                       add-api="saveUser"
                       edit-api="saveUser"
                       remove-api="removeUser"
                       :search-fields="searchFields"
                       :fields="fields"
                       :form-rules="rules"
                       :form-fields="formFields">
    </snail-simple-list-crud>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        keyValues: {
          genders: []
        },
        rules: {
          name: [
            { required: true, message: '必填项', trigger: 'blur' }
          ],
          account: [
            { required: true, message: '必填项', trigger: 'blur' }
          ]
        }
      }
    },
    computed: {
      fields() {
        return [
          {
            name: 'name',
            label: '姓名',
            type: 'string',
            span: 12
          },
          {
            name: 'account',
            label: '账号',
            type: 'string',
            span: 12
          }
        ]
      },
      formFields() {
        return [
          {
            name: 'name',
            label: '姓名',
            type: 'string',
            span: 12
          },
          {
            name: 'account',
            label: '账号',
            type: 'string',
            span: 12
          },
          {
            name: 'pwd',
            label: '密码',
            type: 'string',
            span: 12
          }
        ]
      },
      searchFields() {
        return [
          {
            name: 'keyWord',
            label: '关键字',
            type: 'string'
          }
        ]
      }
    },
    created() {
    },
    methods: {
    }
  }
</script>
