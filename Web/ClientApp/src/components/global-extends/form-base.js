/**
 * 表单通用的方法，所有用到form表单的地方都可以引用此公共js，进行混入，并重写相应的方法已实现自定义功能
 *
 */
export const FormBaseMixin = {
  props: {
    formData: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    // 读取通用配置
    const config = this.$config

    return {
      ComponentSize: config.componentSize(),
      validateFormName: 'ruleForm',
      FormSaveFnName: 'savePersonnelAdditionalInfo',
      FormSaveFnNameAdd: 'savePersonnelAdditionalInfo',
      CustomVerificationErrorMsg: [],
      loadingHandle: {}
    }
  },
  methods: {
    OpenLoading(_text) {
      this.loadingHandle = this.$loading({
        lock: true,
        text: _text || '保存中,请稍等...',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)'
      })
    },
    CloseLoading() {
      this.loadingHandle.close()
    },
    // 用户自定义发送数据包装方法
    makeSendData() {
      return this.formData
    },
    // 用户自定义验证方法
    CustomVerification() {
      this.CustomVerificationErrorMsg = []
      return true
    },
    // 验证失败后的提示信息
    VerificationFailedPromptMessage() {
      var errMsg = this.CustomVerificationErrorMsg.length > 0
        ? '保存失败,导致失败的原因如下：<br>' + this.CustomVerificationErrorMsg.join('<br>')
        : '还有必填项没有填写，请验查！'

      this.$alert(this.$tools.GetErrorHtml(errMsg), this.$t('common.warning'), {
        confirmButtonText: this.$t('common.yes'),
        dangerouslyUseHTMLString: true
      })
    },
    // 表单验证验证前
    beforeSubmitForm() {
    },
    // 表单验证
    submitForm(fn) {
      this.CustomVerificationErrorMsg = []
      this.beforeSubmitForm()
      this.$refs[this.validateFormName].validate(valid => {
        if (valid && this.CustomVerification()) {
          this.SaveForm(fn)
        } else {
          this.VerificationFailedPromptMessage()
          return false
        }
      })
    },
    // 保存
    SaveForm(fn) {
      // 后端根据 Id 来判断是添加还是保存
      const FormSaveFnName = (this.formData.Id === 0) ? this.FormSaveFnNameAdd : this.FormSaveFnName
      const sendData = this.makeSendData()
      this.OpenLoading()
      // 后端根据 Id 来判断是添加还是保存
      this.$client[FormSaveFnName](JSON.stringify(sendData)).then(ret => {
        this.CloseLoading()
        if (ret && ret.code) {
          typeof (fn) === 'function' && fn(ret, sendData)
        } else {
          this.SaveError(ret, sendData, fn)
        }
      }).catch((err) => {
        this.CloseLoading()
        this.ClientRequestError(this.FormSaveFnName)
      })
    },
    // 加载数据错误
    ClientRequestError(FnName) {},
    // 保存失败后的处理
    SaveError(ret) {
      this.$alert(ret.msg, this.$t('common.warning'), {
        confirmButtonText: this.$t('common.yes')
      })
    }
  }
}
