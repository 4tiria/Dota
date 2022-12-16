import React, {useEffect, useState} from 'react';
import {Hero} from "../../models/Hero";
import {useNavigate, useParams} from "react-router-dom";
import {deleteHero, getById, getByName, getTags, updateHero} from "../../api/heroApi";
import "./heroInfo.scss";
import "react-bootstrap";
import {
    IoBanOutline,
    IoCheckmarkOutline,
    IoCloseOutline,
    IoCreateOutline,
    IoRemoveOutline,
    IoTrashBinOutline
} from "react-icons/io5";
import "../../styles/App.scss"
import {Tag} from "../../models/Tag";
import HeroName from "./HeroName";
import {ToCamelCase} from "../../helpers/stringHelper";
import HeroAttribute from "./HeroAttribute";
import HeroTags from "./HeroTags";
import HeroAttackType from "./HeroAttackType";
import Modal from 'react-bootstrap/Modal';

function isNumber(n) {
    return !isNaN(parseInt(n));
}

export const HeroInfo = () => {
        const params = useParams();
        const navigate = useNavigate();
        const [hero, setHero] = useState<Hero>();
        const [editMode, setEdit] = useState(false);
        const [deleteDialogVisible, setDeleteDialogVisible] = useState(false);

        const [heroName, setHeroName] = useState('');
        const [heroAttackType, setHeroAttackType] = useState('');
        const [heroAttribute, setHeroAttribute] = useState('');
        const [tags, setHeroTags] = useState<Tag[]>([]);
        const [tagPull, setTagPull] = useState<Tag[]>([]);

        useEffect(() => {
            if (/\d/.test(params.name)) {
                let number = parseInt(params.name);
                if (isNaN(number)) {
                    //todo: create hero `${}` don't exist
                } else {
                    getById(number).then(response => {
                        setHero(response);
                    });
                }
            } else {
                getByName(params.name).then(response => {
                    setHero(response);
                });
            }
        }, []);

        function startEdit() {
            setEdit(true);
            setHeroName(hero.name);
            setHeroTags(hero.tags);
            setHeroAttribute(hero.mainAttribute);
            setHeroAttackType(hero.attackType);
        }

        function confirm() {
            let flag = true;
            setHero((prevState) => {
                debugger
                const newState = {
                    ...prevState,
                    name: heroName,
                    mainAttribute: heroAttribute,
                    attackType: heroAttackType,
                    tags: tags,
                };
                if (flag) {
                    flag = false;
                    updateHero(newState).then(() => {
                        setEdit(false);
                    });
                }

                return newState;
            });
        }

        function cancel() {
            setEdit(false);
        }

        function callDeleteHero() {
            deleteHero(hero).then(() => navigate("../../heroList"));
        }

        function renderHeroName() {
            return <HeroName
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroName(value)}/>
        }

        function renderHeroAttackType() {
            return <HeroAttackType
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroAttackType(value)}/>
        }

        function renderHeroAttribute() {
            return <HeroAttribute
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroAttribute(value)}/>
        }

        function renderHeroTags() {
            return <HeroTags
                hero={hero}
                editMode={editMode}
                callBackFunction={(tags) => setHeroTags(tags)}/>
        }

        function renderDeleteDialog() {
            return (
                <Modal show={deleteDialogVisible} onHide={() => {}}>
                    <Modal.Dialog>
                        <div>
                            <Modal.Header closeButton>
                                <Modal.Title>Вы точно хотите удалить героя?</Modal.Title>
                            </Modal.Header>
                            <Modal.Footer>
                                {deleteDialogFooter}
                            </Modal.Footer>
                        </div>
                    </Modal.Dialog>
                </Modal>
            )
        }

        const deleteDialogFooter = (
            <div>
                <div className="btn btn-outline-danger" onClick={callDeleteHero}>Удалить</div>
                <div className="btn btn-outline-secondary" onClick={() => setDeleteDialogVisible(false)}>Отмена</div>
            </div>
        )

        return (
            <div className="hero-container">
                <div>
                    <div className="hero-edit-panel">
                        {editMode
                            ?
                            <div className="d-flex justify-content-center">
                                <div className="btn custom-icon" onClick={confirm}>
                                    <IoCheckmarkOutline className="tick"/>
                                </div>
                                <div className="btn custom-icon" onClick={cancel}>
                                    <IoCloseOutline className="cross"/>
                                </div>
                            </div>
                            :
                            <div className="d-flex justify-content-center">
                                {renderDeleteDialog()}
                                <div className="btn custom-icon" onClick={startEdit}><IoCreateOutline/></div>
                                <div className="btn custom-icon" onClick={() => setDeleteDialogVisible(true)}>
                                    <IoTrashBinOutline/></div>
                            </div>
                        }
                    </div>
                    <div className="hero-info">
                        <div className="d-flex justify-content-between">
                            <div className="hero-image"></div>
                            <div className="hero-header">
                                {!!hero
                                    ?
                                    <>
                                        {renderHeroName()}
                                        <div className="d-flex justify-content-center">
                                            {renderHeroAttackType()}
                                            {renderHeroAttribute()}
                                        </div>
                                        {renderHeroTags()}
                                    </>
                                    : <></>}
                                <hr/>
                            </div>
                        </div>
                        <div className="hero-description">

                        </div>
                    </div>
                </div>
            </div>
        )
            ;
    }
;

export default HeroInfo;