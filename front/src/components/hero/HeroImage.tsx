import React, {createRef, DragEventHandler, useEffect, useState} from 'react';
import {ICallBack} from "../interfaces/ICallBack";
import {getTestImage, postImage} from "../../api/heroApi";
import {downloadBlob, getBlobFromBase64} from "../../helpers/blobHelper";
import {Hero} from "../../models/Hero";
import {IEditable} from "../interfaces/IEditable";
import "./heroInfo.scss";

interface IHeroImage extends IEditable {
    hero: Hero;
}

const HeroImage: React.FC<IHeroImage> = ({hero, editMode}) => {
    const [drag, setDrag] = useState<boolean>(false);
    const [file, setPng] = useState<Blob>(null);
    const [filePath, setPngPath] = useState<string>(null);

    useEffect(() => {
        if (hero != null) {
            getTestImage(hero.id).then(data => {
                setPng(data);
            });
        }
    }, [hero]);

    useEffect(() => {
        if (file != null)
            displayFromBlob(file);
    }, [file]);

    function displayFromBlob(blob: Blob) {
        let filePath = window.URL.createObjectURL(blob);
        setPngPath(filePath);
    }

    function changeImage(pngFile: Blob) {
        const imageFile = URL.createObjectURL(pngFile);
        setPng(pngFile);
        setPngPath(imageFile);
        postImage(pngFile, hero.id).then();
    }

    function dragInHandler(event) {
        event.preventDefault();
        setDrag(true);
    }

    function dragOutHandler(event) {
        event.preventDefault();
        setDrag(false);
    }

    function dropHandler(event) {
        event.preventDefault();
        let files = [...event.dataTransfer.files];
        if (files?.length > 0) {
            let suffix = ".png";
            let png = files.find(f => f.name.match(suffix + "$") == suffix);
            if (png != null) {
                changeImage(png);
            }
        }

        setDrag(false);
    }

    return (
        <>
            {hero
                ?
                <>
                    <div className="image-container">
                        {editMode
                            ?
                            <div className="drag-zone w-100 h-100"
                                 onDragOver={dragInHandler}
                                 onDrop={dropHandler}>
                                {drag
                                    ?
                                    <div
                                        className="drop-area d-flex justify-content-center align-items-center w-100 h-100"
                                        onDragLeave={dragOutHandler}>
                                        <div>Перетащите файл</div>
                                    </div>
                                    : <></>
                                }
                            </div>
                            : <></>}
                        <img
                            src={filePath}
                            style={{
                                boxShadow: editMode ? '0px 0px 20px rgba(7, 230, 55, 0.5)' : '',
                                opacity: editMode ? '20%' : '100%',
                                width: '256px',
                                height: '144px',
                                display: 'flex',
                                border: '2px solid tomato',
                            }}/>
                        {editMode ? <div>Перетащите .png</div> : <></>}
                    </div>
                </>
                : <></>
            }

        </>
    );
};

export default HeroImage;


