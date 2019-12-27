<!--
 /**
  *
  * 日期范围选择组件
  *
  *
  * version: 0.1
  *
  * description: 用于日期范围选择，可只选一个日期
  *
  * props:
  *     与 el-data-picker 一致
  *
  * events:
  *     change 用户选择日期后触发
  *
  * changelog
  *     v0.1 2018-12-18 by lanbo | 基本功能
  *     v0.2 2019-01-03 by lanbo | 调整实现方式，修复浮层位置不对问题
  *
  */
-->
<template>
    <div
        class="ex-date-range-picker"
        :class="[visible ? 'is-acitve' : '', disabled ? 'is-disabled' : '']"
        @mousedown.capture.stop="handleMouseDown"
        @mouseup.capture.stop="handleMouseDown"
    >
        <!-- input -->
        <el-date-picker
            v-bind="$attrs"
            :disabled="disabled"
            :prefix-icon="prefixIcon"
            class="ex-date-range-picker__input"
            ref="dateLeft"
            v-model="dateLeft"
            :placeholder="startPlaceholder"
            type="date"
            :default-value="defaultValue[0]"
            clear-icon="el-icon-circle-close"
            @change="handleChange"
        ></el-date-picker>
        <div class="ex-date-range-picker__separator-text" ref="separator">{{ rangeSeparator }}</div>
        <el-date-picker
            v-bind="$attrs"
            :disabled="disabled"
            class="ex-date-range-picker__input ex-date-range-picker__input--right"
            ref="dateRight"
            v-model="dateRight"
            :placeholder="endPlaceholder"
            type="date"
            :default-value="defaultValue[1]"
            clear-icon="el-icon-circle-close"
            @change="handleChange"
        ></el-date-picker>

        <!-- popper -->
        <transition name="el-zoom-in-top">
            <div
                class="ex-date-range-picker__popper-panel el-popper el-picker-panel"
                ref="popperPanel"
                v-show="visible"
            >
                <div class="ex-date-range-picker__popper-panel-body"></div>
            </div>
        </transition>
    </div>
</template>

<script>
export default {
    name: 'ExDateRangePicker',
    props: {
        value: {},
        startPlaceholder: {
            type: String,
            default: ''
        },
        endPlaceholder: {
            type: String,
            default: ''
        },
        rangeSeparator: {
            type: String,
            default: '-'
        },
        defaultValue: {
            type: Array,
            default: () => ['', '']
        },
        prefixIcon: {
            type: String,
            default: 'el-icon-date'
        },
        disabled: {
            type: Boolean,
            default: false
        }
    },
    data() {
        return {
            popperPanel: null,
            visible: false,
            unwatch: null
        };
    },
    computed: {
        dateLeft: {
            get() {
                if (Array.isArray(this.value)) {
                    return this.value[0];
                }
                return '';
            },
            set(val) {
                const value = val || '';
                if (Array.isArray(this.value)) {
                    this.$set(this.value, 0, value);
                } else {
                    this.$emit('input', [value, '']);
                }
            }
        },
        dateRight: {
            get() {
                if (Array.isArray(this.value)) {
                    return this.value[1];
                }
                return '';
            },
            set(val) {
                const value = val || '';
                if (Array.isArray(this.value)) {
                    this.$set(this.value, 1, value);
                } else {
                    this.$emit('input', ['', value]);
                }
            }
        }
    },
    methods: {
        handleChange() {
            this.$emit('change', this.value);
        },
        focus() {
            this.$refs.dateLeft.focus();
        },
        handleMouseDown(e) {
            if (e && e.target && e.target.className.includes('close')) {
                return;
            }

            this.showPicker();
        },
        handleKeydown(event) {
            const keyCode = event.keyCode;

            // ESC
            // eslint-disable-next-line
            if (keyCode === 27) {
                this.hidePicker();
                event.stopPropagation();
                return;
            }

            // Tab
            // eslint-disable-next-line
            if (keyCode === 9) {
                this.hidePicker();
                event.stopPropagation();
                return;
            }
        },
        showPicker() {
            this.$refs.dateLeft.pickerVisible = true;
        },
        hidePicker() {
            this.$refs.dateLeft.pickerVisible = false;
        }
    },
    mounted() {
        try {
            // 将popper添加到body
            document.body.appendChild(this.$refs.popperPanel);
            this.popperPanel = this.$refs.popperPanel;

            // 设置input
            this.$refs.dateLeft.referenceElm = this.$el;
            this.$refs.dateRight.referenceElm = this.$el;

            // 挂载picker
            this.$refs.dateLeft.mountPicker();
            this.$refs.dateRight.mountPicker();

            // 将picker添加到popper中
            this.popperPanel
                .querySelector('.ex-date-range-picker__popper-panel-body')
                .appendChild(this.$refs.dateLeft.popperElm);
            this.popperPanel
                .querySelector('.ex-date-range-picker__popper-panel-body')
                .appendChild(this.$refs.dateRight.popperElm);

            // 设置date的popper
            this.$refs.dateLeft.popperElm = this.popperPanel;
            this.$refs.dateRight.popperElm = this.popperPanel;

            // 取消事件
            this.$refs.dateLeft.picker.$off('pick');
            this.$refs.dateLeft.picker.$off('dodestroy');
            this.$refs.dateRight.picker.$off('pick');
            this.$refs.dateRight.picker.$off('dodestroy');

            // 重新监听
            this.$refs.dateLeft.picker.$on(
                'pick',
                function(date) {
                    this.emitInput(date);
                }.bind(this.$refs.dateLeft)
            );
            this.$refs.dateRight.picker.$on(
                'pick',
                function(date) {
                    this.emitInput(date);
                }.bind(this.$refs.dateRight)
            );

            // 监听picker显示
            this.unwatch = this.$watch(
                () => {
                    if (
                        !this.$refs.dateLeft.pickerVisible &&
                        !this.$refs.dateRight.pickerVisible
                    ) {
                        return false;
                    }

                    return true;
                },
                val => {
                    this.visible = val;
                }
            );
        } catch (error) {
            // do nothing
        }
    },
    beforeDestroy() {
        document.body.removeChild(this.$refs.popperPanel);

        if (this.unwatch) {
            this.unwatch();
        }
    }
};
</script>

<style scoped lang="less">
.ex-date-range-picker {
    display: flex;
    align-items: center;
    white-space: nowrap;
    border: 1px solid #c7c7c9;
    border-radius: 4px;
    position: relative;
    box-sizing: border-box;
    background-color: #fff;

    .el-form-item.is-error & {
        border-color: #fe8b00;
    }

    &__input {
        flex: 1;
        overflow: hidden;
        border-radius: 4px;

        /deep/ .el-input__inner {
            border: none;
            text-align: center;
            margin-top: -1px;
            margin-bottom: -1px;
        }

        &--right {
            /deep/ .el-input__prefix {
                display: none;
            }
        }
    }
    &__separator-text {
        padding: 0 5px;
        font-size: 12px;
    }

    &.is-acitve {
        border: 1px solid #486dd5;
    }
    &.is-disabled {
        background-color: #e6edf9;
        color: #959aa0;
        cursor: not-allowed;
        border-color: #c7c7c9;
    }
}

.ex-date-range-picker__popper-panel {
    min-width: 644px;
    height: 335px;
    position: fixed;
    margin-top: 5px;
    border-radius: 4px;
    overflow: hidden;

    &-body {
        display: flex;
    }

    /deep/ .el-popper {
        transition-duration: 0s !important;
        display: block !important;
        margin: 0;
        border: none;
        border-radius: 0;
    }

    /deep/ .el-popper:first-child {
        border-right: 1px solid #e4e4e4;
    }

    .el-picker-panel__footer {
        padding-right: 10px;
    }
}
</style>
